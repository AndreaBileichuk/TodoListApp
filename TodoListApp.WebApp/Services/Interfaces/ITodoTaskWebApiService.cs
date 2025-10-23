using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoTask;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTaskModel>> GetTodoListTasksAsync(int todoListId);

    Task<TodoTaskDetailsModel?> GetTodoTaskByIdAsync(int todoListId, int todoTaskId);

    Task<bool> CreateTodoTaskAsync(int todoListId, CreateTodoTaskModel createTodoTaskModel);

    Task<bool> UpdateTodoTaskAsync(int todoListId, int todoTaskId, EditTodoTaskModel editTodoTaskDto);

    Task<bool> DeleteTodoTaskAsync(int todoListId, int todoTaskId);

    Task<bool> AssignTaskAsync(int todoListId, int todoTaskId, string assigneeId);
}
