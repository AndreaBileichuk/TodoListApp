using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService _service;

    public TodoListController(ITodoListDatabaseService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoListDto>>> GetAll()
    {
        var todoLists = await this._service.GetTodoLists();
        return this.Ok(todoLists
            .Select(TodoMappers.MapTodoList)
            .ToList());
    }

    [HttpPost]
    public async Task<ActionResult<TodoListDto>> CreateTodoList([FromBody] CreateTodoListDto todoListDtoCreate)
    {
        var todoList = await this._service.AddTodoListAsync(todoListDtoCreate);
        var dto = TodoMappers.MapTodoList(todoList);

        return this.CreatedAtAction(nameof(this.GetAll), new {Id = dto.Id}, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoListDto>> UpdateTodoList(int id, [FromBody] CreateTodoListDto todoListDtoUpdate)
    {
        var todoList = await this._service.UpdateTodoListAsync(id, todoListDtoUpdate);

        if (todoList is null)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoList(todoList));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var result = await this._service.RemoveTodoListAsync(id);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
