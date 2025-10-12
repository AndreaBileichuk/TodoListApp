namespace TodoListApp.WebApi.Models;

/// <summary>
/// This class is used for returning from the controllers.
/// </summary>
public class TodoListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; } = string.Empty;
}
