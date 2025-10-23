using TodoListApp.WebApp.Models.TodoList;

namespace TodoListApp.WebApp.Models.TodoTask;

public class AssignTaskViewModel
{
    public int TodoListId { get; set; }

    public int TodoTaskId { get; set; }

    public string TaskTitle { get; set; } = string.Empty;

    public string CurrentAssigneeId { get; set; } = string.Empty;

    public List<TodoListMemberModel> Members { get; set; } = [];
}
