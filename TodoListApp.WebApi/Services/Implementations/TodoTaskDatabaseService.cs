using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoTaskDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly ApplicationDbContext context;

    public TodoTaskDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TodoTask>> GetTodoTasksAsync(int todoListId, string userId)
    {
        var canAccess = await this.context.TodoListMembers
            .AnyAsync(tm => tm.TodoListId == todoListId && tm.UserId == userId);

        if (!canAccess)
        {
            return new List<TodoTask>();
        }

        return await this.context.TodoTasks.Where(t => t.TodoListId == todoListId).ToListAsync();
    }

    public async Task<TodoTask?> GetTodoTaskByIdAsync(int todoTaskId, string userId)
    {
        return await this.context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == todoTaskId
                                      && t.TodoList!.Members.Any(m => m.UserId == userId));
    }

    public async Task<TodoTask?> AddTodoTaskAsync(int todoListId, CreateTodoTaskDto todoTaskDto, string userId)
    {
        var listExistsForUser = await this.context.TodoListMembers
            .AnyAsync(l => l.TodoListId == todoListId && l.UserId == userId && l.Role == ListRole.Owner);

        if (!listExistsForUser)
        {
            return null;
        }

        TodoTask todoTask = new TodoTask()
        {
            Title = todoTaskDto.Title,
            Description = todoTaskDto.Description,
            DueDate = todoTaskDto.DueDate.ToUniversalTime(),
            TodoListId = todoListId,
            CreatedById = userId,
            AssigneeId = userId
        };

        await this.context.TodoTasks.AddAsync(todoTask);

        await this.context.SaveChangesAsync();

        return todoTask;
    }

    public async Task<TodoTask?> UpdateTodoTaskAsync(int id, UpdateTodoTaskDto updated, string userId)
    {
        var todoTask = await this.context.TodoTasks
            .Include(t => t.TodoList!.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (todoTask is null)
        {
            return null;
        }

        var userMembership = todoTask.TodoList!.Members.FirstOrDefault(m => m.UserId == userId);

        if (userMembership is null)
        {
            return null;
        }

        bool isOwner = userMembership.Role == ListRole.Owner;
        bool isAssignedMember = userMembership.Role == ListRole.Member && todoTask.AssigneeId == userId;

        if (!isOwner && !isAssignedMember)
        {
            return null;
        }

        todoTask.Title = updated.Title;
        todoTask.Description = updated.Description;
        todoTask.Status = updated.Status;
        todoTask.DueDate = updated.DueDate.ToUniversalTime();

        await this.context.SaveChangesAsync();
        return todoTask;
    }

    public async Task<bool> RemoveTodoTaskAsync(int id, string userId)
    {
        TodoTask? todoTask = await this.context.TodoTasks
            .FirstOrDefaultAsync(t => t.Id == id &&
                                      t.TodoList!.Members.Any(m => m.UserId == userId && m.Role == ListRole.Owner));

        if (todoTask is null)
        {
            return false;
        }

        this.context.Remove(todoTask);
        await this.context.SaveChangesAsync();
        return true;
    }
}
