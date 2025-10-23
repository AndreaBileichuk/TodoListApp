using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Dashboard;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

[Authorize]
public class DashboardController : BaseApiController
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboardData()
    {
        var dashboardData = await this._service.GetDashboardDataAsync(this.UserId);
        return this.Ok(dashboardData);
    }
}
