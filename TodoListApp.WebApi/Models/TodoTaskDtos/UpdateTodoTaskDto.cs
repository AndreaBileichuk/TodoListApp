using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoTaskDtos;

public class UpdateTodoTaskDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public TodoTaskStatus Status { get; set; }

    public DateTime DueDate { get; set; }
}
