using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoTaskDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoTaskDatabaseService
{
    Task<List<TodoTask>> GetTodoTasksAsync(int todoListId, string userId);

    Task<TodoTask?> GetTodoTaskByIdAsync(int todoTaskId, string userId);

    Task<TodoTask?> AddTodoTaskAsync(int todoListId, CreateTodoTaskDto todoTaskDto, string userId);

    Task<TodoTask?> UpdateTodoTaskAsync(int id, UpdateTodoTaskDto updated, string userId);

    Task<bool> RemoveTodoTaskAsync(int id, string userId);
}
