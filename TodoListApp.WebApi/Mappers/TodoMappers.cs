using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoListCommentDtos;
using TodoListApp.WebApi.Models.TodoListDtos;
using TodoListApp.WebApi.Models.TodoTaskDtos;

namespace TodoListApp.WebApi.Mappers;

public static class TodoMappers
{
    public static TodoListDto MapTodoList(TodoList todo) => new TodoListDto() {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
    };

    public static TodoTaskDto MapTodoTask(TodoTask todoTask) => new TodoTaskDto()
    {
        Id = todoTask.Id,
        Title = todoTask.Title,
        Status = todoTask.Status,
        IsOverdue = todoTask.DueDate < DateTime.UtcNow && todoTask.Status != TodoTaskStatus.Completed,
        AssigneeId = todoTask.AssigneeId
    };

    public static TodoTaskDetailsDto MapTodoTaskDetail(TodoTask todoTask) => new()
    {
        Id = todoTask.Id,
        Title = todoTask.Title,
        Description = todoTask.Description,
        Status = todoTask.Status,
        CreatedAt = todoTask.CreatedAt,
        DueDate = todoTask.DueDate,
        AssigneeId = todoTask.AssigneeId,
        IsOverdue = todoTask.DueDate < DateTime.UtcNow && todoTask.Status != TodoTaskStatus.Completed
    };

    public static AssignedTaskDto MapTodoTaskToAssigned(TodoTask todoTask) => new()
    {
        Id = todoTask.Id,
        Title = todoTask.Title,
        Description = todoTask.Description,
        Status = todoTask.Status,
        CreatedAt = todoTask.CreatedAt,
        DueDate = todoTask.DueDate,
        AssigneeId = todoTask.AssigneeId,
        TodoListId = todoTask.TodoListId,
        IsOverdue = todoTask.DueDate < DateTime.UtcNow && todoTask.Status != TodoTaskStatus.Completed
    };

    public static TodoListCommentDto MapTodoListCommentTodoListCommentDto(TodoListComment dto) => new()
    {
        Id = dto.Id,
        Text = dto.Text,
        CreatedAt = dto.CreatedAt,
        UserId = dto.User?.Id,
        UserName = dto.User?.UserName,
        AvatarUrl = dto.User?.AvatarUrl
    };
}
