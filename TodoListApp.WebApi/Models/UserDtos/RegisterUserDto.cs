using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.UserDtos;

public class RegisterUserDto
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
