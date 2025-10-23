using TodoListApp.WebApp.Enums;

namespace TodoListApp.WebApp.Models.TodoTask;

public class TodoTaskModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public TodoTaskStatus Status { get; set; }

    public bool IsOverdue { get; set; }

    public string AssigneeId { get; set; } = string.Empty;
}
