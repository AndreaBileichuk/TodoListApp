using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string query,
        [FromQuery] int todoListId)
    {
        var users = await this._userService.SearchUsersAsync(query, todoListId, this.UserId);
        return this.Ok(users);
    }
}
