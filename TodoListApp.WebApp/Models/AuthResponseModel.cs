namespace TodoListApp.WebApp.Models;

public class AuthResponseModel
{
    public string? AccessToken { get; set; }
}

public class AuthServiceResponse
{
    public bool IsSuccess { get; set; }
    public string? AccessToken { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }
}

public class ErrorResponseDto
{
    public Dictionary<string, List<string>> Errors { get; set; } = [];
}
