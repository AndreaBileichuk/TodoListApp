using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models;

public class UpdateTaskStatusDto
{
    public TodoTaskStatus TodoTaskStatus { get; set; }
}
