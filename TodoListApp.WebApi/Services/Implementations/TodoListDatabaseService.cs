using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TodoList>> GetTodoLists()
    {
        return await this.context.TodoLists.ToListAsync();
    }

    public async Task<TodoList> AddTodoListAsync(CreateTodoListDto dto)
    {
        TodoList list = new TodoList() { Title = dto.Title, Description = dto.Description };

        await this.context.TodoLists.AddAsync(list);

        await this.context.SaveChangesAsync();

        return list;
    }

    public async Task<bool> RemoveTodoListAsync(int id)
    {
        TodoList? todoList = await this.context.TodoLists.FindAsync(id);

        if (todoList == null)
        {
            return false;
        }

        this.context.Remove(todoList);
        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task<TodoList?> UpdateTodoListAsync(int id, CreateTodoListDto updated)
    {
        TodoList? todoList = await this.context.TodoLists.FindAsync(id);

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
