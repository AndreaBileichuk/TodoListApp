using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoListDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
public class TodoListController : BaseApiController
{
    private readonly ITodoListDatabaseService _todoListService;
    private readonly ITodoListMemberService _todoListMemberService;

    public TodoListController(ITodoListDatabaseService todoListService, ITodoListMemberService todoListMemberService)
    {
        this._todoListService = todoListService;
        this._todoListMemberService = todoListMemberService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoListDto>>> GetAll()
    {
        var todoLists = await this._todoListService.GetTodoLists(this.UserId);
        return this.Ok(todoLists
            .Select(TodoMappers.MapTodoList)
            .ToList());
    }

    [HttpGet("{todoListId}")]
    public async Task<ActionResult<TodoListDetailsDto>> GetById(int todoListId)
    {
        var todoList = await this._todoListService.GetTodoListById(todoListId, this.UserId);

        if (todoList is null)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoList(todoList));
    }

    [HttpGet("details/{todoListId}")]
    public async Task<ActionResult<TodoListDetailsDto>> GetDetails(int todoListId)
    {
        var todoListDetails = await this._todoListService.GetTodoListDetails(todoListId, this.UserId);

        if (todoListDetails is null)
        {
            return this.NotFound();
        }

        return this.Ok(todoListDetails);
    }

    [HttpPost]
    public async Task<ActionResult<TodoListDto>> CreateTodoList([FromBody] CreateTodoListDto todoListDtoCreate)
    {
        var todoList = await this._todoListService.AddTodoListAsync(todoListDtoCreate, this.UserId);
        var dto = TodoMappers.MapTodoList(todoList);

        return this.CreatedAtAction(nameof(this.GetAll), new {Id = dto.Id}, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoListDto>> UpdateTodoList(int id, [FromBody] CreateTodoListDto todoListDtoUpdate)
    {
        var todoList = await this._todoListService.UpdateTodoListAsync(id, todoListDtoUpdate, this.UserId);

        if (todoList is null)
        {
            return this.NotFound();
        }

        return this.Ok(TodoMappers.MapTodoList(todoList));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var result = await this._todoListService.RemoveTodoListAsync(id, this.UserId);

        if (!result)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }

    // Handling members
    [HttpPost("{todoListId}/members")]
    public async Task<IActionResult> AddTodoListMember(int todoListId, AddTodoListMemberDto dto)
    {
        var result = await this._todoListMemberService.AddMemberAsync(todoListId, this.UserId, dto.Email);

        if (!result)
        {
            return this.BadRequest("Something is wrong");
        }

        return this.NoContent();
    }

    [HttpDelete("{todoListId}/members/{memberIdToDelete}")]
    public async Task<IActionResult> RemoveTodoListMember(int todoListId, string memberIdToDelete)
    {
        var result = await this._todoListMemberService.RemoveMemberAsync(todoListId, this.UserId, memberIdToDelete);

        if (!result)
        {
            return this.BadRequest("Something is wrong");
        }

        return this.NoContent();
    }
}
