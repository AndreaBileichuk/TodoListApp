namespace TodoListApp.WebApi.Models.UserDtos;

public class UserDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; } = null;

    public string Email { get; set; } = string.Empty;
};
