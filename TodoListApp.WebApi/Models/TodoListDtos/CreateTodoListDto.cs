namespace TodoListApp.WebApi.Models.TodoListDtos;

/// <summary>
/// This class is used by the controller to create and update a todoList. It is a binding model.
/// </summary>
public class CreateTodoListDto
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
}
