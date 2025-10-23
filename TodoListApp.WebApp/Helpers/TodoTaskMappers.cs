using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoTask;

namespace TodoListApp.WebApp.Helpers;

public static class TodoTaskMappers
{
    public static EditTodoTaskModel MapTodoTaskDetailsToEdit(TodoTaskDetailsModel model) => new()
    {
        Id = model.Id,
        Title = model.Title,
        Description = model.Description,
        Status = model.Status,
        CreatedAt = model.CreatedAt,
        DueDate = model.DueDate,
        IsOverdue = model.IsOverdue
    };
}
