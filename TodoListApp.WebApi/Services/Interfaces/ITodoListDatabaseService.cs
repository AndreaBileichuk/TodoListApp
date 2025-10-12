using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoListDatabaseService
{
    Task<List<TodoList>> GetTodoLists();

    Task<TodoList> AddTodoListAsync(CreateTodoListDto dto);

    Task<bool> RemoveTodoListAsync(int id);

    Task<TodoList?> UpdateTodoListAsync(int id, CreateTodoListDto updated);
}
