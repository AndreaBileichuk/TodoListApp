using TodoListApp.WebApi.Data.Entities;

namespace TodoListApp.WebApi.Models.TodoListCommentDtos;

public class TodoListCommentDto
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; } = string.Empty;

    public string? UserName { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }
}
