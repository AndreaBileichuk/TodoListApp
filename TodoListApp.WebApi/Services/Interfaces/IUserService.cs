using TodoListApp.WebApi.Models.UserDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> SearchUsersAsync(string query, int todoListId, string currentUserId);
}
