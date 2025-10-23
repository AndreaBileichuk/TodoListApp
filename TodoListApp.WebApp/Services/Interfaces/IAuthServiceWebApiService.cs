using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface IAuthServiceWebApiService
{
    public Task<string?> LoginAsync(LoginUserModel model);

    public Task<AuthServiceResponse> RegisterAsync(RegisterUserModel model);

    public Task<UserModel?> AuthMe(string accessToken);
}
