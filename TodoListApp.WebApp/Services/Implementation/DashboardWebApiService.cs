using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class DashboardWebApiService : IDashboardWebApiService
{
    private readonly IClientAuth _clientAuth;

    public DashboardWebApiService(IClientAuth clientAuth)
    {
        this._clientAuth = clientAuth;
    }

    public async Task<DashBoardModel?> GetDashboardDataAsync()
    {
        var client = _clientAuth.CreateAuthorizedClient();

        try
        {
            return await client.GetFromJsonAsync<DashBoardModel>("api/dashboard", JsonConfigurations.GetJsonSerializerOptions());
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
