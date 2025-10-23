using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models.UserDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<List<UserDto>> SearchUsersAsync(string query, int todoListId, string currentUserId)
    {
        var lowerQuery = query.ToLower();

        var existingMemberIds = await this._context.TodoListMembers
            .Where(m => m.TodoListId == todoListId)
            .Select(m => m.UserId)
            .ToListAsync();

        var users = await this._context.Users
            .Where(u => (u.Email!.ToLower().Contains(lowerQuery) || u.UserName!.ToLower().Contains(lowerQuery)) &&
                        !existingMemberIds.Contains(u.Id) &&
                        u.Id != currentUserId)
            .OrderBy(u => u.UserName)
            .Take(10)
            .Select(u => new UserDto() { Id = u.Id, UserName = u.UserName!, Email = u.Email!, AvatarUrl = u.AvatarUrl })
            .ToListAsync();

        return users;
    }
}
