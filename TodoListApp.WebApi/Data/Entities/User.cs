using Microsoft.AspNetCore.Identity;
using TodoListApp.WebApi.Data.Enums;

namespace TodoListApp.WebApi.Data.Entities;

public class User : IdentityUser
{
    public string? AvatarUrl { get; set; } = null;

    public List<TodoListMember> TodoListMemberships { get; set; } = [];

    public List<TodoTask> CreatedTasks { get; set; } = [];

    public List<TodoTask> AssignedTasks { get; set; } = [];
}
