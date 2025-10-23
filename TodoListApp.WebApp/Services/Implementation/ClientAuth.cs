using System.Security.Claims;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class ClientAuth : IClientAuth
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientAuth(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        this._httpClientFactory = httpClientFactory;
        this._httpContextAccessor = httpContextAccessor;
    }

    public HttpClient CreateAuthorizedClient()
    {
        var token = this._httpContextAccessor.HttpContext?.User.FindFirstValue("access_token");

        var client = this._httpClientFactory.CreateClient("WebApi");

        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new ("Bearer", token);
        }

        return client;
    }
}
