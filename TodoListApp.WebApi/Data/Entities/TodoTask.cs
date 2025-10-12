using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TodoTaskStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime DueDate { get; set; }

    public int AssigneeId { get; set; }

    public int TodoListId { get; set; }

    public TodoList? TodoList { get; set; }
}
