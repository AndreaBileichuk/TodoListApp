using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models.TodoListCommentDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoListCommentService : ITodoListCommentService
{
    private readonly ApplicationDbContext _context;
    public TodoListCommentService(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<List<TodoListCommentDto>> GetTodoListCommentsAsync(int todoListId, string userId)
    {
        var todoList = await this._context.TodoLists
            .Include(tl => tl.Members)
            .FirstOrDefaultAsync(t => t.Id == todoListId);

        if (todoList is null)
        {
            return new List<TodoListCommentDto>();
        }

        var isUserAMember = todoList.Members.Any(m => m.UserId == userId);

        if (!isUserAMember)
        {
            return new List<TodoListCommentDto>();
        }

        return await this._context.TodoListComments
            .Where(c => c.TodoListId == todoListId)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => TodoMappers.MapTodoListCommentTodoListCommentDto(c))
            .ToListAsync();
    }

    public async Task<bool> AddCommentAsync(int todoListId, string userId, string text)
    {
        var todoList = await this._context.TodoLists
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == todoListId);

        if (todoList is null)
        {
            return false;
        }

        var canAccess = todoList.Members.Any(m => m.UserId == userId);

        if (!canAccess)
        {
            return false;
        }

        TodoListComment newComment = new TodoListComment()
        {
            Text = text,
            TodoListId = todoListId,
            UserId = userId
        };

        this._context.Add(newComment);
        await this._context.SaveChangesAsync();

        return true;
    }
}
