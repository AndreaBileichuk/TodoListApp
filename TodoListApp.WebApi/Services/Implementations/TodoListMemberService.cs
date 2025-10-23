using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoListMemberService : ITodoListMemberService
{
    private readonly ApplicationDbContext _context;

    public TodoListMemberService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<bool> AddMemberAsync(int todoListId, string ownerUserId, string userEmail)
    {
        var listExistsAndIsOwner = await this._context.TodoLists
            .Include(todoList => todoList.Members)
            .AnyAsync(
                t => t.Id == todoListId && t.Members.Any(m => m.UserId == ownerUserId && m.Role == ListRole.Owner));

        if (!listExistsAndIsOwner)
        {
            return false;
        }

        var user = await this._context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user is null)
        {
            return false;
        }

        var isAlreadyAMember = await this._context.TodoListMembers
            .AnyAsync(m => m.TodoListId == todoListId && m.UserId == user.Id);

        if (isAlreadyAMember)
        {
            return false;
        }

        var newTodoListMember = new TodoListMember
        {
            TodoListId = todoListId, UserId = user.Id, Role = ListRole.Member
        };

        await this._context.TodoListMembers.AddAsync(newTodoListMember);

        await this._context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveMemberAsync(int todoListId, string ownerUserId, string userIdToRemove)
    {
        var memberToRemove = await this._context.TodoListMembers
            .FirstOrDefaultAsync(m => m.TodoListId == todoListId &&
                                      m.UserId == userIdToRemove);

        if (memberToRemove is null)
        {
            return false;
        }

        var isOwner = await this._context.TodoListMembers
            .AnyAsync(m => m.TodoListId == todoListId &&
                           m.UserId == ownerUserId &&
                           m.Role == ListRole.Owner);

        if (!isOwner)
        {
            return false;
        }

        if (memberToRemove.UserId == ownerUserId && memberToRemove.Role == ListRole.Owner)
        {
            return false;
        }

        var assignedTasks = await this._context.TodoTasks
            .Where(t => t.TodoListId == todoListId && t.AssigneeId == memberToRemove.UserId)
            .ToListAsync();

        foreach (var task in assignedTasks)
        {
            task.AssigneeId = ownerUserId;
        }

        this._context.TodoListMembers.Remove(memberToRemove);
        await this._context.SaveChangesAsync();

        return true;
    }
}
