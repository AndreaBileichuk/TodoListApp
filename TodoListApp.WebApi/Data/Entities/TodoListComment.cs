namespace TodoListApp.WebApi.Data.Entities;

public class TodoListComment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;

    public User? User { get; set; } = null;

    public int TodoListId { get; set; }

    public TodoList? TodoList { get; set; } = null;
}
