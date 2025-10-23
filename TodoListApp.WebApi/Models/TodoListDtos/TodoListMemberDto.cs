using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Models.TodoListDtos;

public class TodoListMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public ListRole Role { get; set; }
}
