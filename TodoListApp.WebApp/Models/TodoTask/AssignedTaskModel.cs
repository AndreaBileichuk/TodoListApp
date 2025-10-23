using TodoListApp.WebApp.Enums;

namespace TodoListApp.WebApp.Models.TodoTask;

public class AssignedTaskModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TodoTaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime DueDate { get; set; }

    public string AssigneeId { get; set; } = string.Empty;

    public bool IsOverdue { get; set; }

    public int TodoListId { get; set; }
}
