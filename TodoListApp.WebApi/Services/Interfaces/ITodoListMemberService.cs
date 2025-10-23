namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoListMemberService
{
    Task<bool> AddMemberAsync(int todoListId, string ownerUserId, string userEmail);
    Task<bool> RemoveMemberAsync(int todoListId, string ownerUserId, string userIdToRemove);
}
