using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardWebApiService _webApiService;

    public HomeController(IDashboardWebApiService webApiService)
    {
        this._webApiService = webApiService;
    }

    public IActionResult Index()
    {
        var userIdentity = this.User.Identity;
        if (userIdentity != null && userIdentity.IsAuthenticated)
        {
            return this.RedirectToAction("DashBoard");
        }

        return this.View();
    }

    [Authorize]
    public async Task<IActionResult> DashBoard()
    {
        DashBoardModel? model = await this._webApiService.GetDashboardDataAsync();

        if (model is null)
        {
            this.TempData["ErrorMessage"] = "Could not load dashboard data. The service might be temporarily unavailable.";

            return this.View("Error");
        }

        return this.View(model);
    }
}
