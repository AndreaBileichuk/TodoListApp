using System.Security.Claims;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementation;

public class AuthServiceWebApiService : IAuthServiceWebApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthServiceWebApiService(IHttpClientFactory httpClientFactory)
    {
        this._httpClientFactory = httpClientFactory;
    }

    public async Task<string?> LoginAsync(LoginUserModel model)
    {
        var client = this._httpClientFactory.CreateClient("WebApi");
        var response = await client.PostAsJsonAsync("api/auth/login", model);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseModel>();

        return authResponse?.AccessToken;
    }

    public async Task<AuthServiceResponse> RegisterAsync(RegisterUserModel model)
    {
        var client = this._httpClientFactory.CreateClient("WebApi");
        var response = await client.PostAsJsonAsync("/api/auth/register", model);

        if (response.IsSuccessStatusCode)
        {
            var authResponse = await response.Content.ReadFromJsonAsync<AuthServiceResponse>();
            return new AuthServiceResponse { IsSuccess = true, AccessToken = authResponse?.AccessToken };
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
        return new AuthServiceResponse { IsSuccess = false, Errors = errorResponse?.Errors };
    }

    public async Task<UserModel?> AuthMe(string accessToken)
    {
        var userClient = this._httpClientFactory.CreateClient("WebApi");
        userClient.DefaultRequestHeaders.Authorization = new("Bearer", accessToken);
        return await userClient.GetFromJsonAsync<UserModel>("api/auth/me");
    }
}
