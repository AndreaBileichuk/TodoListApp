using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoListDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoListDatabaseService
{
    Task<List<TodoList>> GetTodoLists(string userId);

    Task<TodoList?> GetTodoListById(int todoListId, string userId);

    Task<TodoListDetailsDto?> GetTodoListDetails(int todoListId, string userId);

    Task<TodoList> AddTodoListAsync(CreateTodoListDto dto, string userId);

    Task<bool> RemoveTodoListAsync(int id, string userId);

    Task<TodoList?> UpdateTodoListAsync(int id, CreateTodoListDto updated, string userId);
}
