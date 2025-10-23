using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models.TodoTaskDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface IAssignedTasksDatabaseService
{
    Task<List<TodoTask>> GetAssignedTasksAsync(string userId, TodoTaskStatus? status = null, string? sortBy= null, string? sortDirection = null);

    Task<bool> UpdateAssignedTaskStatusAsync(string userId, int taskId, TodoTaskStatus status);

    Task<bool> AssignTaskToUser(string listCreatorId, int taskId, string assigneeId);
}
