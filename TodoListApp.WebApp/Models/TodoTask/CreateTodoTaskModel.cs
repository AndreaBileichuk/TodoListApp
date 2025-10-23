using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models.TodoTask;

public class CreateTodoTaskModel
{
    [Required(ErrorMessage = "The task title cannot be empty.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "The description is too long (max 255 characters).")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "A due date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }
}
