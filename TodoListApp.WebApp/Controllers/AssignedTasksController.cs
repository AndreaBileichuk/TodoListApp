using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models.TodoTask;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("[controller]")]
public class AssignedTasksController : Controller
{
    private readonly IAssignedTasksWebApiService _assignedTasksWebApiService;

    public AssignedTasksController(IAssignedTasksWebApiService assignedTasksWebApiService)
    {
        this._assignedTasksWebApiService = assignedTasksWebApiService;
    }

    public async Task<ActionResult> Index(
        [FromQuery] string? status = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null)
    {
        var assignedTasks = await this._assignedTasksWebApiService.GetAssignedTasksAsync(status, sortBy, sortDirection);

        if (assignedTasks is null)
        {
            this.TempData[StatusMessages.ErrorMessage] = "Не вдалося завантажити призначені завдання. Сервіс може бути тимчасово недоступний.";
            return this.View("Error");
        }

        this.ViewData["CurrentSortBy"] = sortBy;
        this.ViewData["CurrentSortDirection"] = sortDirection;
        this.ViewData["CurrentStatus"] = status;

        this.ViewData["NextSortDirectionDueDate"] = (sortBy == "duedate" && sortDirection == "asc") ? "desc" : "asc";
        this.ViewData["NextSortDirectionTitle"] = (sortBy == "title" && sortDirection == "asc") ? "desc" : "asc";

        return this.View(assignedTasks);
    }

    [HttpPatch("tasks/{taskId}/update-status")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] UpdateTaskStatusDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        var result = await this._assignedTasksWebApiService.UpdateAssignedTaskStatusAsync(taskId, dto.TodoTaskStatus);

        if (!result)
        {
            return this.NotFound(new { message = "Не вдалося оновити статус. Завдання не знайдено або у вас немає прав." });
        }

        return this.NoContent();
    }
}
