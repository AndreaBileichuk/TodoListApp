namespace TodoListApp.WebApp.Services.Interfaces;

public interface IClientAuth
{
    public HttpClient CreateAuthorizedClient();
}
