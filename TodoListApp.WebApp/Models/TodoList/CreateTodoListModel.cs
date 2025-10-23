using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.TodoList;

public class CreateTodoListModel
{
    [Required(ErrorMessage = "The task title cannot be empty.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "The description is too long (max 1000 characters).")]
    public string? Description { get; set; } = string.Empty;
}
