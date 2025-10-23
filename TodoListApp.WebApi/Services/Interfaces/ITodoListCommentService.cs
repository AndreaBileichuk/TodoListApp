using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Models.TodoListCommentDtos;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoListCommentService
{
    Task<List<TodoListCommentDto>> GetTodoListCommentsAsync(int todoListId, string userId);

    Task<bool> AddCommentAsync(int todoListId, string userId, string text);
}
