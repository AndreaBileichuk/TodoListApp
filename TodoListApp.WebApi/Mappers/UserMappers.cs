using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.UserDtos;

namespace TodoListApp.WebApi.Mappers;

public static class UserMappers
{
    public static UserDto UserToUserDto(User user) => new()
    {
        Id = user.Id,
        UserName = user.UserName!,
        Email = user.Email!,
        AvatarUrl = user.AvatarUrl,
    };

    public static LoginUserDto LoginUserDto(RegisterUserDto userDto) => new()
    {
        Email = userDto.Email,
        Password = userDto.Password
    };
}
