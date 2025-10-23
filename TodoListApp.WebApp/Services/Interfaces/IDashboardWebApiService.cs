using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface IDashboardWebApiService
{
    Task<DashBoardModel?> GetDashboardDataAsync();
}
