using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models.TodoListDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly ApplicationDbContext context;

    public TodoListDatabaseService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TodoList>> GetTodoLists(string userId)
    {
        return await this.context.TodoLists.Where(tl => tl.Members.Any(m => m.UserId == userId)).ToListAsync();
    }

    public async Task<TodoList?> GetTodoListById(int todoListId, string userId)
    {
        var todoList = await this.context.TodoLists
            .Where(l => l.Id == todoListId && l.Members.Any(m => m.UserId == userId))
            .FirstOrDefaultAsync();

        return todoList;
    }

    public async Task<TodoListDetailsDto?> GetTodoListDetails(int todoListId, string userId)
    {
        var todoList = await this.context.TodoLists
            .Include(l => l.Members)
            .ThenInclude(m => m.User)
            .Include(m => m.TodoTasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == todoListId);

        if (todoList == null)
        {
            return null;
        }

        var userMembership = todoList.Members.FirstOrDefault(m => m.UserId == userId);

        if (userMembership == null)
        {
            return null;
        }

        var detailsDto = new TodoListDetailsDto
        {
            Id = todoList.Id,
            Title = todoList.Title,
            Description = todoList.Description,

            CurrentUserRole = userMembership.Role,

            Members = todoList.Members.Select(m => new TodoListMemberDto
            {
                UserId = m.UserId,
                UserName = m.User?.UserName,
                AvatarUrl = m.User?.AvatarUrl,
                Role = m.Role
            }).ToList(),

            Tasks = todoList.TodoTasks.Select(TodoMappers.MapTodoTask).ToList()
        };

        return detailsDto;
    }

    public async Task<TodoList> AddTodoListAsync(CreateTodoListDto dto, string userId)
    {
        TodoList list = new TodoList() { Title = dto.Title, Description = dto.Description };

        var member = new TodoListMember
        {
            TodoList = list,
            UserId = userId,
            Role = ListRole.Owner
        };

        await this.context.TodoListMembers.AddAsync(member);
        await this.context.SaveChangesAsync();

        return list;
    }

    public async Task<bool> RemoveTodoListAsync(int id, string userId)
    {
        var todoList = await this.context.TodoLists
            .FirstOrDefaultAsync(l => l.Id == id &&
                                            l.Members.Any(m => m.UserId == userId && m.Role == ListRole.Owner));

        if (todoList == null)
        {
            return false;
        }

        this.context.Remove(todoList);
        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<TodoList?> UpdateTodoListAsync(int id, CreateTodoListDto updated, string userId)
    {
        var todoList = await this.context.TodoLists
            .FirstOrDefaultAsync(l => l.Id == id &&
                                      l.Members.Any(m => m.UserId == userId && m.Role == ListRole.Owner));

        if (todoList == null)
        {
            return null;
        }

        todoList.Title = updated.Title;
        todoList.Description = updated.Description;

        await this.context.SaveChangesAsync();

        return todoList;
    }
}
