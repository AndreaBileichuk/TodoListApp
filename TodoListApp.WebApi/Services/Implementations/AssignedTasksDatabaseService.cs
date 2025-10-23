using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models.TodoTaskDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class AssignedTasksDatabaseService : IAssignedTasksDatabaseService
{
    private readonly ApplicationDbContext _context;

    public AssignedTasksDatabaseService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<List<TodoTask>> GetAssignedTasksAsync(string userId, TodoTaskStatus? status = null, string? sortBy = null, string? sortDirection = null)
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

    public async Task<bool> UpdateAssignedTaskStatusAsync(string userId, int taskId, TodoTaskStatus status)
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

    public async Task<bool> AssignTaskToUser(string listCreatorId, int taskId, string assigneeId)
    {
        var todoTask = await this._context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == taskId &&
                                      t.TodoList!.Members.Any(m => m.UserId == listCreatorId && m.Role == ListRole.Owner));

        if (todoTask is null)
        {
            return false;
        }

        var assigneeIsMember = await this._context.TodoListMembers
            .AnyAsync(m => m.TodoListId == todoTask.TodoListId && m.UserId == assigneeId);

        if (!assigneeIsMember)
        {
            return false;
        }

        todoTask.AssigneeId = assigneeId;
        await this._context.SaveChangesAsync();

        return true;
    }
}
