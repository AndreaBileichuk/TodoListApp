using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoTaskDtos;

public class CreateTodoTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }
}
