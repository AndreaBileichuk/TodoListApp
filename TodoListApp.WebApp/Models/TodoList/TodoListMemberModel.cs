using TodoListApp.WebApp.Enums;

namespace TodoListApp.WebApp.Models.TodoList;

public class TodoListMemberModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public ListRole Role { get; set; }
}
