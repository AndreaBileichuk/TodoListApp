using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class UserProfileModel
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }

    [Display(Name = "Нове ім'я користувача")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Ім'я має бути від 3 до 50 символів")]
    public string? NewUserName { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Старий пароль")]
    public string? OldPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Новий пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути не менше 6 символів")]
    public string? NewPassword { get; set; }
}
