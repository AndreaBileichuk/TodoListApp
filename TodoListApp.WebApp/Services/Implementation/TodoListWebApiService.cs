using System.Text.Json;
using System.Text.Json.Serialization;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoList;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly IClientAuth _clientAuth;

    public TodoListWebApiService(IClientAuth clientAuth)
    {
        this._clientAuth = clientAuth;
    }

    public async Task<IEnumerable<TodoListModel>> GetAllAsync()
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        return await client.GetFromJsonAsync<IEnumerable<TodoListModel>>("/api/todolist") ?? [];
    }

    public async Task<TodoListModel?> GetTodoListById(int todoListId)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.GetAsync($"api/todolist/{todoListId}");
        return await response.Content.ReadFromJsonAsync<TodoListModel>() ?? null;
    }

    public async Task<TodoListDetailsModel?> GetTodoListDetails(int todoListId)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.GetAsync($"api/todolist/details/{todoListId}");
        return await response.Content.ReadFromJsonAsync<TodoListDetailsModel>(JsonConfigurations.GetJsonSerializerOptions()) ?? null;
    }

    public async Task<bool> CreateTodoListAsync(CreateTodoListModel createTodoListModel)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync("api/todolist", createTodoListModel);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateTodoListAsync(int todoListId, EditTodoListModel editTodoListModel)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.PutAsJsonAsync($"api/todolist/{todoListId}", editTodoListModel);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTodoListAsync(int todoListId)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.DeleteAsync($"api/todolist/{todoListId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddMemberAsync(int todoListId, string email)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.PostAsJsonAsync($"api/todolist/{todoListId}/members", new { Email = email });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveMemberAsync(int todoListId, string memberIdToRemove)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        var response = await client.DeleteAsync($"api/todolist/{todoListId}/members/{memberIdToRemove}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<UserModel>> SearchUsersAsync(int todoListId, string query)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        try
        {
            var response = await client.GetAsync($"api/users/search?query={Uri.EscapeDataString(query)}&todoListId={todoListId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<UserModel>>() ?? new List<UserModel>();
        }
        catch(HttpRequestException)
        {
            return new List<UserModel>();
        }
    }

    public async Task<List<TodoListCommentModel>?> GetTodoListCommentsAsync(int todoListId)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        try
        {
            var response = await client.GetAsync($"api/todolist/{todoListId}/comments");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TodoListCommentModel>>() ?? [];
        }
        catch(HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> AddTodoListCommentsAsync(int todoListId, string text)
    {
        var client = this._clientAuth.CreateAuthorizedClient();
        try
        {
            var response = await client.PostAsJsonAsync($"api/todolist/{todoListId}/comments", new { text });
            return response.IsSuccessStatusCode;
        }
        catch(HttpRequestException)
        {
            return false;
        }
    }
}
