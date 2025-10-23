using TodoListApp.WebApi.Models.TodoListDtos;
using TodoListApp.WebApi.Models.TodoTaskDtos;

namespace TodoListApp.WebApi.Models.Dashboard;

public class DashboardDto
{
    public int MyListsCount { get; set; }
    public int AssignedToMeCount { get; set; }
    public int OverdueTasksCount { get; set; }
    public List<TodoListDto> TodoLists { get; set; } = [];
}
