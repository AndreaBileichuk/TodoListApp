using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoTaskDtos;

public class UpdateTaskStatusDto
{
    public TodoTaskStatus TodoTaskStatus { get; set; }
}
