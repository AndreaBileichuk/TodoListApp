using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.TodoListCommentDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Route("api/todolist/{todoListId}/comments")]
public class TodoListCommentController : BaseApiController
{
    private readonly ITodoListCommentService _todoListCommentService;

    public TodoListCommentController(ITodoListCommentService todoListCommentService)
    {
        this._todoListCommentService = todoListCommentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoListCommentDto>>> GetComments(int todoListId)
    {
        return this.Ok(await this._todoListCommentService.GetTodoListCommentsAsync(todoListId, this.UserId));
    }

    [HttpPost]
    public async Task<ActionResult<TodoListCommentDto>> AddComment(int todoListId, [FromBody] AddCommentRequestDto dto)
    {
        var result = await this._todoListCommentService.AddCommentAsync(todoListId, this.UserId, dto.Text);

        if (!result)
        {
            return this.BadRequest(new { message = "Could not add comment. Ensure the list exists and you are a member." });
        }

        return this.NoContent();
    }
}
