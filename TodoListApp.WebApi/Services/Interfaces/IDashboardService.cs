using TodoListApp.WebApi.Models.Dashboard;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardDataAsync(string userId);
}
