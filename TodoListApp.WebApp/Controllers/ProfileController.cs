using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
[Route("[controller]")]
public class ProfileController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new UserProfileModel()
        {
            UserName = this.User.Identity?.Name ?? "Unknown",
            Email = this.User?.FindFirstValue(ClaimTypes.Email) ?? "No email",
            AvatarUrl = this.User?.FindFirstValue("AvatarUrl") ?? DefaultAvatars.DefaultAvatar
        };

        return this.View(model);
    }
}
