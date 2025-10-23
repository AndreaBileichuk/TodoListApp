using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoTaskDtos;

public class AssignedTaskDto
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
