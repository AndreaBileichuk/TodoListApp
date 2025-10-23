using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = null;

    public TodoTaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime DueDate { get; set; }

    public string CreatedById { get; set; } = string.Empty;

    public User? CreatedBy { get; set; }

    public string AssigneeId { get; set; } = string.Empty;

    public User? Assignee { get; set; }

    public int TodoListId { get; set; }

    public TodoList? TodoList { get; set; }
}
