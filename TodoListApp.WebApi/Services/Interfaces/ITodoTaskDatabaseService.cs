using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoTaskDatabaseService
{
    Task<List<TodoTask>> GetTodoTasksAsync(int todoListId);

    Task<TodoTask?> GetTodoTaskByIdAsync(int todoTaskId);

    Task<TodoTask?> AddTodoTaskAsync(int todoListId, CreateTodoTaskDto todoTaskDto);

    Task<TodoTask?> UpdateTodoTaskAsync(int id, UpdateTodoTaskDto updated);

    Task<bool> RemoveTodoTaskAsync(int id);
}
