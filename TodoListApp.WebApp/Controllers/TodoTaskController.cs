using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoTask;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("todolist")]
public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService _todoTaskWebApiService;
    private readonly ITodoListWebApiService _todoListWebApiService;

    private const string ActionRedirect = "Details";
    private const string ControllerRedirect = "TodoList";

    public TodoTaskController(ITodoTaskWebApiService todoTaskWebApiService, ITodoListWebApiService todoListWebApiService)
    {
        this._todoTaskWebApiService = todoTaskWebApiService;
        this._todoListWebApiService = todoListWebApiService;
    }

    // Creating a todoTask
    [HttpGet("{todoListId}/tasks/create")]
    public IActionResult Create(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var createTodoTaskModel = new CreateTodoTaskModel();
        createTodoTaskModel.DueDate = DateTime.UtcNow;
        this.ViewData["todoListId"] = todoListId;
        return this.PartialView("_CreateTaskPartial", createTodoTaskModel);
    }

    [HttpPost("{todoListId}/tasks/create")]
    public async Task<IActionResult> Create(int todoListId, CreateTodoTaskModel createTodoTaskModel)
    {
        if(!this.ModelState.IsValid)
        {
            this.ViewData["todoListId"] = todoListId;
            return this.PartialView("_CreateTaskPartial", createTodoTaskModel);
        }

        var result = await this._todoTaskWebApiService.CreateTodoTaskAsync(todoListId, createTodoTaskModel);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to update the task!";
        }

        return this.RedirectToAction(ActionRedirect, ControllerRedirect, new { todoListId });
    }

    // Editing a todoTask
    [HttpGet("{todoListId}/tasks/edit/{todoTaskId}")]
    public async Task<IActionResult> Edit(int todoListId, int todoTaskId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoTaskDetails = await this._todoTaskWebApiService.GetTodoTaskByIdAsync(todoListId, todoTaskId);

        if (todoTaskDetails == null)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to find the task!";
            return this.RedirectToAction(ActionRedirect, ControllerRedirect, new { todoListId });
        }

        this.ViewData["todoListId"] = todoListId;
        return this.PartialView("_EditTaskPartial", TodoTaskMappers.MapTodoTaskDetailsToEdit(todoTaskDetails));
    }

    [HttpPost("{todoListId}/tasks/edit/{todoTaskId}")]
    public async Task<IActionResult> Edit(int todoListId, int todoTaskId, EditTodoTaskModel editTodoTaskModel)
    {
        if (!this.ModelState.IsValid)
        {
            this.ViewData["todoListId"] = todoListId;
            return this.PartialView("_EditTaskPartial", editTodoTaskModel);
        }

        var result = await this._todoTaskWebApiService.UpdateTodoTaskAsync(todoListId, todoTaskId, editTodoTaskModel);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to update the task!";
        }

        return this.RedirectToAction(ActionRedirect, ControllerRedirect, new { todoListId });
    }

    // DELETE SECTION
    [HttpGet("{todoListId}/tasks/delete/{todoTaskId}")]
    public async Task<IActionResult> Delete(int todoListId, int todoTaskId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoTask = await this._todoTaskWebApiService.GetTodoTaskByIdAsync(todoListId, todoTaskId);

        if (todoTask is not null)
        {
            this.ViewData["todoListId"] = todoListId;
            return this.PartialView("_DeleteTodoTaskPartial", todoTask);
        }

        this.TempData[StatusMessages.ErrorMessage] = "Couldn't find a todo task";
        return this.RedirectToAction(ActionRedirect, ControllerRedirect, new { todoListId });
    }

    [HttpPost("{todoListId}/tasks/delete/{todoTaskId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int todoListId, int todoTaskId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var result = await this._todoTaskWebApiService.DeleteTodoTaskAsync(todoListId, todoTaskId);

        if (result)
        {
            this.TempData[StatusMessages.SuccessMessage] = "Task deleted successfully!";
        }
        else
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to delete the task.";
        }

        return this.RedirectToAction(ActionRedirect, ControllerRedirect, new { todoListId });
    }

    // Assigning a task to a user
    [HttpGet("{todoListId}/tasks/{todoTaskId}/assign")]
    public async Task<IActionResult> Assign(int todoListId, int todoTaskId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoListDetails = await this._todoListWebApiService.GetTodoListDetails(todoListId);
        var todoTaskDetails = await this._todoTaskWebApiService.GetTodoTaskByIdAsync(todoListId, todoTaskId);

        if(todoListDetails is null || todoTaskDetails is null)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Не вдалося знайти список або завдання.";
            return this.RedirectToAction("Details", "TodoList", new { todoListId });
        }

        var viewModel = new AssignTaskViewModel
        {
            TodoListId = todoListId,
            TodoTaskId = todoTaskId,
            TaskTitle = todoTaskDetails.Title,
            Members = todoListDetails.Members,
            CurrentAssigneeId = todoTaskDetails.AssigneeId
        };

        return this.PartialView("_AssignTaskPartial", viewModel);
    }

    [HttpPost("{todoListId}/tasks/{todoTaskId}/assign")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignTaskToUser(int todoListId, int todoTaskId, [FromForm] string assigneeId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var result = await this._todoTaskWebApiService.AssignTaskAsync(todoListId, todoTaskId, assigneeId);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Не вдалося призначити завдання. Перевірте права доступу або чи користувач є учасником списку.";
        }
        else
        {
            this.TempData[StatusMessages.SuccessMessage] = "Завдання успішно призначено!";
        }

        return this.RedirectToAction("Details", "TodoList", new { todoListId });
    }
}
