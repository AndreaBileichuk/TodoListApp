using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.UserDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface IAuthDatabaseService
{
    public Task<string?> Register(RegisterUserDto userDto);

    public Task<string?> Login(LoginUserDto userDto);

    public Task<User?> GetCurrentUser(string userId);
}
