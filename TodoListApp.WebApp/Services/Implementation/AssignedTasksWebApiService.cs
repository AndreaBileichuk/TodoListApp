using System.Text;
using TodoListApp.WebApp.Enums;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models.TodoTask;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class AssignedTasksWebApiService : IAssignedTasksWebApiService
{
    private readonly IClientAuth _clientAuth;

    public AssignedTasksWebApiService(IClientAuth clientAuth)
    {
        this._clientAuth = clientAuth;

    }
    public async Task<List<AssignedTaskModel>?> GetAssignedTasksAsync(string? status = null, string? sortBy = null, string? sortDirection = null)
    {
        var client = this._clientAuth.CreateAuthorizedClient();

        var urlBuilder = new StringBuilder("api/assignedtasks");
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(status))
        {
            queryParams.Add($"status={Uri.EscapeDataString(status)}");
        }
        if (!string.IsNullOrEmpty(sortBy))
        {
            queryParams.Add($"sortBy={Uri.EscapeDataString(sortBy)}");
        }
        if (!string.IsNullOrEmpty(sortDirection))
        {
            queryParams.Add($"sortDirection={Uri.EscapeDataString(sortDirection)}");
        }

        if (queryParams.Any())
        {
            urlBuilder.Append('?').Append(string.Join('&', queryParams));
        }

        var requestUrl = urlBuilder.ToString();

        try
        {
            var response = await client.GetAsync(requestUrl);

            if(response.IsSuccessStatusCode)
            {
                var tasks = await response.Content.ReadFromJsonAsync<List<AssignedTaskModel>>(JsonConfigurations.GetJsonSerializerOptions());
                return tasks ?? new List<AssignedTaskModel>();
            }

            return null;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAssignedTaskStatusAsync(int taskId, TodoTaskStatus status)
    {
        var client = this._clientAuth.CreateAuthorizedClient();

        var content = JsonContent.Create(new { todoTaskStatus = status });

        try
        {
            var response = await client.PatchAsync($"api/assignedtasks/{taskId}", content);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Network error updating task status: {ex.Message}");
            return false;
        }
    }
}
