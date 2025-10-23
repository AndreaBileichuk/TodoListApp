using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoList;

namespace TodoListApp.WebApp.Helpers;

public static class TodoListMappers
{
    public static EditTodoListModel MapTodoListToEditTodoListModel(TodoListModel model) => new ()
    {
        Title = model.Title,
        Description = model.Description,
    };
}
