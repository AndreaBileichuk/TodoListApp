using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected string UserId => this.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
