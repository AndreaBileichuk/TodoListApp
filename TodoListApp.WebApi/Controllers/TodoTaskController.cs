using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoTaskDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
[Route("api/todolist/{todoListId}/tasks")]
public class TodoTaskController : BaseApiController
{
    private readonly ITodoTaskDatabaseService _taskService;
    private readonly IAssignedTasksDatabaseService _assignedTasksDatabase;

    public TodoTaskController(ITodoTaskDatabaseService taskService, IAssignedTasksDatabaseService assignedTasksDatabase)
    {
        this._taskService = taskService;
        this._assignedTasksDatabase = assignedTasksDatabase;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAll(int todoListId)
    {
        var todoTasks = await this._taskService.GetTodoTasksAsync(todoListId, this.UserId);
        return this.Ok(todoTasks.Select(TodoMappers.MapTodoTask).ToList());
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<TodoTaskDetailsDto>> GetById(int todoListId, int taskId)
    {
        var todoTask = await this._taskService.GetTodoTaskByIdAsync(taskId, this.UserId);

        if (todoTask == null || todoTask.TodoListId != todoListId)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoTaskDetail(todoTask));
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskDto>> CreateTodoTask(int todoListId, CreateTodoTaskDto createTodoTask)
    {
        var todoTask = await this._taskService.AddTodoTaskAsync(todoListId, createTodoTask, this.UserId);

        if(todoTask is null)
        {
            return this.BadRequest($"TodoList with ID {todoListId} was not found.");
        }

        return this.CreatedAtAction(nameof(this.GetById), new { todoListId = todoTask.TodoListId, taskId = todoTask.Id }, TodoMappers.MapTodoTask(todoTask));
    }

    [HttpPut("{taskId}")]
    public async Task<ActionResult<TodoTaskDto>> UpdateTodoTask(int todoListId, int taskId, [FromBody] UpdateTodoTaskDto updateTodoTaskDto)
    {
        var todoTask = await this._taskService.UpdateTodoTaskAsync(taskId, updateTodoTaskDto, this.UserId);

        if (todoTask is null || todoTask.TodoListId != todoListId)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoTask(todoTask));
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTodoTask(int todoListId, int taskId)
    {
        var result = await this._taskService.RemoveTodoTaskAsync(taskId, this.UserId);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    [HttpPut("{taskId}/assign")]
    public async Task<IActionResult> AssignTask(int todoListId, int taskId, [FromBody] AssignTaskDto dto)
    {
        var result = await this._assignedTasksDatabase.AssignTaskToUser(this.UserId, taskId, dto.AssigneeId);

        if (!result)
        {
            return this.NotFound("Task not found, permission denied, or assignee is not a list member.");
        }

        return this.NoContent();
    }
}
