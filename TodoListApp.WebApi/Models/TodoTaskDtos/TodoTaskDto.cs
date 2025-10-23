using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoTaskDtos;

public class TodoTaskDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public TodoTaskStatus Status { get; set; }

    public bool IsOverdue { get; set; }

    public string AssigneeId { get; set; } = string.Empty;
}
