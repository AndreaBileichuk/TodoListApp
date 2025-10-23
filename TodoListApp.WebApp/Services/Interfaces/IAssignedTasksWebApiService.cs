using TodoListApp.WebApp.Enums;
using TodoListApp.WebApp.Models.TodoTask;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface IAssignedTasksWebApiService
{
    Task<List<AssignedTaskModel>?> GetAssignedTasksAsync(string? status = null, string? sortBy = null, string? sortDirection = null);

    Task<bool> UpdateAssignedTaskStatusAsync(int taskId, TodoTaskStatus status);
}
