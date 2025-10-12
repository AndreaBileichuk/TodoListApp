using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoTaskDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TodoTask>> GetTodoTasksAsync(int todoListId)
    {
        var todoTasks = this.context.TodoTasks.Where(t => t.TodoListId == todoListId);
        return await todoTasks.ToListAsync();
    }

    public async Task<TodoTask?> GetTodoTaskByIdAsync(int todoTaskId)
    {
        var todoTasks = await this.context.TodoTasks.FindAsync(todoTaskId);
        return todoTasks;
    }

    public async Task<TodoTask?> AddTodoTaskAsync(int todoListId, CreateTodoTaskDto todoTaskDto)
    {
        if (await this.context.TodoLists.FindAsync(todoListId) == null)
        {
            return null;
        }

        int assigneeId = 1; // MOCK ============== //
        TodoTask todoTask = new TodoTask()
        {
            Title = todoTaskDto.Title,
            Description = todoTaskDto.Description,
            DueDate = todoTaskDto.DueDate,
            TodoListId = todoListId,
            AssigneeId = assigneeId
        };

        await this.context.TodoTasks.AddAsync(todoTask);

        await this.context.SaveChangesAsync();

        return todoTask;
    }

    public async Task<TodoTask?> UpdateTodoTaskAsync(int id, UpdateTodoTaskDto updated)
    {
        TodoTask? todoTask = await this.context.TodoTasks.FindAsync(id);

        if (todoTask == null)
        {
            return null;
        }

        todoTask.Title = updated.Title;
        todoTask.Description = updated.Description;
        todoTask.Status = updated.Status;
        todoTask.DueDate = updated.DueDate;
        todoTask.AssigneeId = updated.AssigneeId;

        await this.context.SaveChangesAsync();

        return todoTask;
    }

    public async Task<bool> RemoveTodoTaskAsync(int id)
    {
        TodoTask? todoTask = await this.context.TodoTasks.FindAsync(id);

        if (todoTask == null)
        {
            return false;
        }

        this.context.Remove(todoTask);
        await this.context.SaveChangesAsync();

        return true;
    }
}
