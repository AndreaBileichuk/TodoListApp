using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoList;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoListModel>> GetAllAsync();

    Task<TodoListModel?> GetTodoListById(int todoListId);

    Task<TodoListDetailsModel?> GetTodoListDetails(int todoListId);

    Task<bool> CreateTodoListAsync(CreateTodoListModel createTodoListModel);

    Task<bool> UpdateTodoListAsync(int todoListId, EditTodoListModel editTodoListModel);

    Task<bool> DeleteTodoListAsync(int todoListId);

    Task<bool> AddMemberAsync(int todoListId, string email);

    Task<bool> RemoveMemberAsync(int todoListId, string memberIdToRemove);

    Task<List<UserModel>> SearchUsersAsync(int todoListId, string query);

    Task<List<TodoListCommentModel>?> GetTodoListCommentsAsync(int todoListId);

    Task<bool> AddTodoListCommentsAsync(int todoListId, string text);
}
