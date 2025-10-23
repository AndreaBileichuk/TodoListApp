using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class RegisterUserModel
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; } = null;
}
