using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Mappers;

public static class TodoMappers
{
    public static TodoListDto MapTodoList(TodoList todo) => new TodoListDto() {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description
    };

    public static TodoTaskDto MapTodoTask(TodoTask todoTask) => new TodoTaskDto()
    {
        Id = todoTask.Id,
        Title = todoTask.Title,
        Status = todoTask.Status,
        IsOverdue = todoTask.DueDate < DateTime.UtcNow && todoTask.Status != TodoTaskStatus.Completed
    };

    public static TodoTaskDetailsDto MapTodoTaskDetail(TodoTask todoTask) => new TodoTaskDetailsDto()
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
}
