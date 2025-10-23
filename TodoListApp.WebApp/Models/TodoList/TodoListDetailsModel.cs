using TodoListApp.WebApp.Enums;
using TodoListApp.WebApp.Models.TodoTask;

namespace TodoListApp.WebApp.Models.TodoList;

public class TodoListDetailsModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public ListRole CurrentUserRole { get; set; }
    public List<TodoListMemberModel> Members { get; set; } = [];
    public List<TodoTaskModel> Tasks { get; set; } = [];
}
