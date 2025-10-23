using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class TodoListMember
{
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    public int TodoListId { get; set; }
    public TodoList? TodoList { get; set; }

    public ListRole Role { get; set; }
}
