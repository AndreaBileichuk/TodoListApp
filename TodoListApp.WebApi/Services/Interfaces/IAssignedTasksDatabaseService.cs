using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface IAssignedTasksDatabaseService
{
    Task<List<TodoTask>> GetAssignedTasksAsync(int userId, TodoTaskStatus? status = null, string? sortBy= null, string? sortDirection = null);
    Task<bool> UpdateAssignedTaskStatusAsync(int userId, int taskId, TodoTaskStatus status);
}
