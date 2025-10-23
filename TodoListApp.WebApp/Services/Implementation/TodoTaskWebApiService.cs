using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoTask;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class TodoTaskWebApiService : ITodoTaskWebApiService
{
    private readonly IClientAuth _clientAuth;

    public TodoTaskWebApiService(IClientAuth clientAuth)
    {
        this._clientAuth = clientAuth;
    }

    public async Task<IEnumerable<TodoTaskModel>> GetTodoListTasksAsync(int todoListId)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().GetAsync($"api/todolist/{todoListId}/tasks");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<TodoTaskModel>>(JsonConfigurations.GetJsonSerializerOptions()) ?? [];
    }

    public async Task<TodoTaskDetailsModel?> GetTodoTaskByIdAsync(int todoListId, int todoTaskId)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().GetAsync($"api/todolist/{todoListId}/tasks/{todoTaskId}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<TodoTaskDetailsModel>(JsonConfigurations.GetJsonSerializerOptions());
    }

    public async Task<bool> CreateTodoTaskAsync(int todoListId, CreateTodoTaskModel createTodoTaskModel)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().PostAsJsonAsync($"api/todolist/{todoListId}/tasks/",
            createTodoTaskModel,
            options:JsonConfigurations.GetJsonSerializerOptions());
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateTodoTaskAsync(int todoListId, int todoTaskId, EditTodoTaskModel editTodoTaskDto)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().PutAsJsonAsync($"api/todolist/{todoListId}/tasks/{todoTaskId}",
            editTodoTaskDto,
            options:JsonConfigurations.GetJsonSerializerOptions());
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTodoTaskAsync(int todoListId, int todoTaskId)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().DeleteAsync($"api/todolist/{todoListId}/tasks/{todoTaskId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AssignTaskAsync(int todoListId, int todoTaskId, string assigneeId)
    {
        var response = await this._clientAuth.CreateAuthorizedClient().PutAsJsonAsync($"api/todolist/{todoListId}/tasks/{todoTaskId}/assign", new
        {
            AssigneeId = assigneeId
        });
        return response.IsSuccessStatusCode;
    }
}
