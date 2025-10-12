using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class AssignedTasksDatabaseService : IAssignedTasksDatabaseService
{
    private readonly TodoListDbContext _context;

    public AssignedTasksDatabaseService(TodoListDbContext context)
    {
        this._context = context;
    }

    public async Task<List<TodoTask>> GetAssignedTasksAsync(int userId, TodoTaskStatus? status = null, string? sortBy = null, string? sortDirection = null)
    {
        var query = this._context.TodoTasks.Where(t => t.AssigneeId == userId);

        query = status is null ? query.Where(t => t.Status != TodoTaskStatus.Completed) : query.Where(t => t.Status == status);

        bool isDescending = sortDirection?.ToLower() == "desc";

        switch (sortBy?.ToLower())
        {
            case "duedate":
                query = isDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate);
                break;
            case "title":
                query = isDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title);
                break;
            default:
                query = query.OrderBy(t => t.CreatedAt);
                break;
        }

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateAssignedTaskStatusAsync(int userId, int taskId, TodoTaskStatus status)
    {
        var todoTask = await this._context.TodoTasks.FindAsync(taskId);

        if (todoTask is null || todoTask.AssigneeId != userId)
        {
            return false;
        }

        todoTask.Status = status;

        await this._context.SaveChangesAsync();

        return true;
    }
}
