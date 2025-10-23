using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Exceptions;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models.UserDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthDatabaseService _service;

    public AuthController(IAuthDatabaseService service)
    {
        this._service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto userDto)
    {
        try
        {
            var accessToken = await this._service.Register(userDto);
            if (accessToken is null)
            {
                return this.Unauthorized();
            }

            return this.Ok(new { accessToken});
        }
        catch (AuthInvalidException e)
        {
            // Create a dictionary to hold structured errors
            var errorDictionary = new Dictionary<string, List<string>>();

            foreach (var error in e.Errors)
            {
                string fieldKey;

                if (error.Code.Contains("Password"))
                {
                    fieldKey = "Password";
                }
                else if (error.Code.Contains("Email") || error.Code.Contains("InvalidEmail"))
                {
                    fieldKey = "Email";
                }
                else if (error.Code.Contains("UserName") || error.Code.Contains("DuplicateUserName"))
                {
                    fieldKey = "UserName";
                }
                else
                {
                    fieldKey = ""; // This targets the 'ModelOnly' summary
                }

                if (!errorDictionary.ContainsKey(fieldKey))
                {
                    errorDictionary[fieldKey] = new List<string>();
                }
                errorDictionary[fieldKey].Add(error.Description);
            }

            return this.BadRequest(new { errors = errorDictionary });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
    {
        var accessToken = await this._service.Login(loginUserDto);
        if (accessToken is null)
        {
            return this.Unauthorized();
        }

        return this.Ok(new {accessToken});
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await this._service.GetCurrentUser(this.UserId);
        if (user is null)
        {
            return this.BadRequest();
        }

        return this.Ok(UserMappers.UserToUserDto(user));
    }
}
