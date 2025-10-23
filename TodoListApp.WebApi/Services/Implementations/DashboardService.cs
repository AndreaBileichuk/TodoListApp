using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models.Dashboard;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(string userId)
    {
        var listsCount = await this._context.TodoListMembers
            .CountAsync(tm => tm.UserId == userId);

        var assignedTasks = await this._context.TodoTasks
            .CountAsync(t => t.AssigneeId == userId && t.Status != TodoTaskStatus.Completed);

        var overdueTasks = await this._context.TodoTasks
            .CountAsync(t => t.AssigneeId == userId &&
                             t.Status != TodoTaskStatus.Completed &&
                             t.DueDate < DateTime.UtcNow);

        var todoLists = await this._context.TodoLists
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .Take(3)
            .Select(t => TodoMappers.MapTodoList(t))
            .ToListAsync();

        return new DashboardDto()
        {
            MyListsCount = listsCount,
            AssignedToMeCount = assignedTasks,
            OverdueTasksCount = overdueTasks,
            TodoLists = todoLists
        };
    }
}
