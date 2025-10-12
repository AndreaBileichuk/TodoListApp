using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Data.Enums;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/todolists/{todoListId}/tasks")]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskDatabaseService _service;

    public TodoTaskController(ITodoTaskDatabaseService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoTaskDto>>> GetAll(int todoListId)
    {
        var todoTasks = await this._service.GetTodoTasksAsync(todoListId);
        return this.Ok(todoTasks.Select(TodoMappers.MapTodoTask).ToList());
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<TodoTaskDetailsDto>> GetById(int todoListId, int taskId)
    {
        var todoTask = await this._service.GetTodoTaskByIdAsync(taskId);

        if (todoTask == null || todoTask.TodoListId != todoListId)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoTaskDetail(todoTask));
    }

    [HttpPost]
    public async Task<ActionResult<TodoTaskDto>> CreateTodoTask(int todoListId, CreateTodoTaskDto createTodoTask)
    {
        var todoTask = await this._service.AddTodoTaskAsync(todoListId, createTodoTask);

        if(todoTask is null)
        {
            return this.BadRequest($"TodoList with ID {todoListId} was not found.");
        }

        return this.CreatedAtAction(nameof(this.GetById), new { todoListId = todoTask.TodoListId, taskId = todoTask.Id }, TodoMappers.MapTodoTask(todoTask));
    }

    [HttpPut("{taskId}")]
    public async Task<ActionResult<TodoTaskDto>> UpdateTodoTask(int todoListId, int taskId, [FromBody] UpdateTodoTaskDto updateTodoTaskDto)
    {
        var todoTask = await this._service.UpdateTodoTaskAsync(taskId, updateTodoTaskDto);

        if (todoTask is null || todoTask.TodoListId != todoListId)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoTask(todoTask));
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTodoTask(int todoListId, int taskId)
    {
        var result = await this._service.RemoveTodoTaskAsync(taskId);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
