using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models.TodoTaskDtos;

namespace TodoListApp.WebApi.Models.TodoListDtos;

public class TodoListDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public ListRole CurrentUserRole { get; set; }
    public List<TodoListMemberDto> Members { get; set; } = [];
    public List<TodoTaskDto> Tasks { get; set; } = [];
}
