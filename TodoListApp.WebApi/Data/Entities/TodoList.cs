using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class TodoList
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;

    public List<TodoTask> TodoTasks { get; set; } = [];

    public List<TodoListMember> Members { get; set; } = [];

    public List<TodoListComment> Comments { get; set; } = [];
}
