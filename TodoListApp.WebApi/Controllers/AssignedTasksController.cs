using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models.TodoTaskDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

public class AssignedTasksController : BaseApiController
{
    private readonly IAssignedTasksDatabaseService _service;

    public AssignedTasksController(IAssignedTasksDatabaseService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDetailsDto>>> GetAssignedTasks(
        [FromQuery]TodoTaskStatus? status = null,
        [FromQuery]string? sortBy = null,
        [FromQuery]string? sortDirection = null)
    {
        var assignedTasks = await this._service.GetAssignedTasksAsync(this.UserId, status, sortBy, sortDirection);
        return this.Ok(assignedTasks.Select(TodoMappers.MapTodoTaskToAssigned));
    }

    [HttpPatch("{taskId}")]
    public async Task<ActionResult> UpdateAssignedTaskStatus(int taskId, [FromBody] UpdateTaskStatusDto status)
    {
        var result = await this._service.UpdateAssignedTaskStatusAsync(this.UserId, taskId, status.TodoTaskStatus);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
