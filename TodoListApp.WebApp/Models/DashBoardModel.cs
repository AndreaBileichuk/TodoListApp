using TodoListApp.WebApp.Models.TodoList;

namespace TodoListApp.WebApp.Models;

public class DashBoardModel
{
    public int MyListsCount { get; set; }
    public int AssignedToMeCount { get; set; }
    public int OverdueTasksCount { get; set; }
    public List<TodoListModel> TodoLists { get; set; } = [];
}
