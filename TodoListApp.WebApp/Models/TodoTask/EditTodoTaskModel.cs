using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApp.Enums;

namespace TodoListApp.WebApp.Models.TodoTask;

public class EditTodoTaskModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The task title cannot be empty.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "The description is too long (max 255 characters).")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Please select a status.")]
    public TodoTaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "A due date is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Due Date")]
    public DateTime DueDate { get; set; }

    public bool IsOverdue { get; set; }
}
