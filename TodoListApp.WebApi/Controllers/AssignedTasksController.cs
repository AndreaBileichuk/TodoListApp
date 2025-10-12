using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/{userId}")]
public class AssignedTasksController : ControllerBase
{
    private readonly IAssignedTasksDatabaseService _service;

    public AssignedTasksController(IAssignedTasksDatabaseService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAssignedTasks(
        int userId,
        [FromQuery]TodoTaskStatus? status = null,
        [FromQuery]string? sortBy = null,
        [FromQuery]string? sortDirection = null)
    {
        var assignedTasks = await this._service.GetAssignedTasksAsync(userId, status, sortBy, sortDirection);
        return this.Ok(assignedTasks.Select(TodoMappers.MapTodoTask).ToList());
    }

    [HttpPatch("tasks/{taskId}")]
    public async Task<ActionResult> UpdateAssignedTaskStatus(int userId, int taskId, [FromBody] UpdateTaskStatusDto status)
    {
        var result = await this._service.UpdateAssignedTaskStatusAsync(userId, taskId, status.TodoTaskStatus);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
