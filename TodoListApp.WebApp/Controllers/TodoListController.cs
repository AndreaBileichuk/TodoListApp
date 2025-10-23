using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Enums;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Models.TodoList;
using TodoListApp.WebApp.Services;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("[controller]")]
public class TodoListController : Controller
{
    private readonly ITodoListWebApiService _service;
    private const string DetailsPage = "Details";
    private const string IndexPage = "Index";

    public TodoListController(ITodoListWebApiService service)
    {
        this._service = service;
    }

    public async Task<IActionResult> Index()
    {
        var todoLists = await this._service.GetAllAsync();
        return this.View(todoLists);
    }

    [HttpGet("{todoListId}/details")]
    public async Task<IActionResult> Details(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();

        }
        var todoListDetails = await this._service.GetTodoListDetails(todoListId);
        return this.View(todoListDetails);
    }

    // Creating a new todoList
    [HttpGet("create")]
    public IActionResult Create()
    {
        CreateTodoListModel createTodoListModel = new CreateTodoListModel();
        return this.PartialView("_CreateTodoListPartial", createTodoListModel);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTodoListModel createTodoListModel)
    {
        if(!this.ModelState.IsValid)
        {
            return this.PartialView("_CreateTodoListPartial", createTodoListModel);
        }

        var result = await this._service.CreateTodoListAsync(createTodoListModel);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to create a todo list";
        }

        return this.RedirectToAction(IndexPage);
    }

    // Editing a todoList
    [HttpGet("edit/{todoListId}")]
    public async Task<IActionResult> Edit(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoList = await this._service.GetTodoListById(todoListId);

        if (todoList is not null)
        {
            this.ViewData["todoListId"] = todoList.Id;
            return this.PartialView("_EditTodoListPartial", TodoListMappers.MapTodoListToEditTodoListModel(todoList));
        }

        this.TempData[StatusMessages.ErrorMessage] = "Couldn't find a todo list";
        return this.RedirectToAction(IndexPage);
    }

    [HttpPost("edit/{todoListId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int todoListId, EditTodoListModel editTodoListModel)
    {
        if(!this.ModelState.IsValid)
        {
            this.ViewData["todoListId"] = todoListId;
            return this.PartialView("_EditTodoListPartial", editTodoListModel);
        }

        var result = await this._service.UpdateTodoListAsync(todoListId, editTodoListModel);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Couldn't update the todo list";
        }

        return this.RedirectToAction(DetailsPage, new { todoListId });
    }

    // Deleting a todoList
    [HttpGet("delete/{todoListId}")]
    public async Task<IActionResult> Delete(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoList = await this._service.GetTodoListById(todoListId);

        if (todoList is not null)
        {
            this.ViewData["todoListId"] = todoList.Id;
            return this.PartialView("_DeleteTodoListPartial", todoList);
        }

        this.TempData[StatusMessages.ErrorMessage] = "Couldn't find a todo list";
        return this.RedirectToAction(IndexPage);
    }

    [HttpPost("delete/{todoListId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var result = await this._service.DeleteTodoListAsync(todoListId);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Todo list was not deleted!";
        }
        else
        {
            this.TempData[StatusMessages.SuccessMessage] = "Todo list deleted successfully!";
        }
        return this.RedirectToAction(IndexPage);
    }

    [HttpGet("{todoListId}/manage-members")]
    public async Task<IActionResult> ManageMembers(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var todoListDetails = await this._service.GetTodoListDetails(todoListId);

        if (todoListDetails is null)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Todo list not found.";
            return this.RedirectToAction(IndexPage);
        }

        if (todoListDetails.CurrentUserRole != ListRole.Owner)
        {
            this.TempData[StatusMessages.ErrorMessage] = "You do not have permission to manage members for this list.";

            return this.RedirectToAction(DetailsPage, new { todoListId });
        }

        // Pass the full details model to the view
        return this.View(todoListDetails);
    }

    [HttpPost("{todoListId}/manage-members/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMember(int todoListId, [FromForm] string emailToAdd)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        if (string.IsNullOrWhiteSpace(emailToAdd))
        {
            this.TempData[StatusMessages.ErrorMessage] = "Email cannot be empty.";
            return this.RedirectToAction("ManageMembers", new { todoListId });
        }

        var result = await this._service.AddMemberAsync(todoListId, emailToAdd);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to add member. Check if the user exists or is already a member.";
        }
        else
        {
            this.TempData[StatusMessages.SuccessMessage] = $"User {emailToAdd} added successfully.";
        }

        return this.RedirectToAction("ManageMembers", new { todoListId });
    }

    [HttpPost("{todoListId}/manage-members/remove/{memberIdToRemove}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveMember(int todoListId, string memberIdToRemove)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var result = await this._service.RemoveMemberAsync(todoListId, memberIdToRemove);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Failed to remove member. You might not have permission, or they are the last owner.";
        }
        else
        {
            this.TempData[StatusMessages.SuccessMessage] = "Member removed successfully.";
        }

        return this.RedirectToAction("ManageMembers", new { todoListId });
    }

    [HttpGet("{todoListId}/manage-members/search-users")]
    public async Task<IActionResult> SearchUsers(int todoListId, [FromQuery] string query)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var users = await this._service.SearchUsersAsync(todoListId, query);
        return this.Json(users);
    }

    [HttpGet("{todoListId}/comments")]
    public async Task<IActionResult> GetComments(int todoListId)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        var comments = await this._service.GetTodoListCommentsAsync(todoListId);

        if (comments is null)
        {
            return this.StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "Не вдалося завантажити коментарі. Сервіс недоступний." });
        }

        return this.Json(comments);
    }

    [HttpPost("{todoListId}/add-comment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int todoListId, [FromForm] string commentText)
    {
        if (!this.ModelState.IsValid)
        {
            return this.NotFound();
        }

        if (string.IsNullOrWhiteSpace(commentText))
        {
            this.TempData[StatusMessages.ErrorMessage] = "Коментар не може бути порожнім.";
            return this.RedirectToAction(DetailsPage, new { todoListId });
        }

        var result = await this._service.AddTodoListCommentsAsync(todoListId, commentText);

        if (!result)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Не вдалося додати коментар. Перевірте права доступу або чи існує список.";
        }
        else
        {
            this.TempData[StatusMessages.SuccessMessage] = "Коментар додано.";
        }

        return this.RedirectToAction(DetailsPage, new { todoListId });
    }
}
