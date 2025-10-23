namespace TodoListApp.WebApp.Models;

public class UserModel
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; } = null;

    public string Email { get; set; } = string.Empty;
}
