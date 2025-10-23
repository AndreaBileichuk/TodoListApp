using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Helpers;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthServiceWebApiService _authServiceWebApiService;

    public AuthController(IAuthServiceWebApiService authServiceWebApiService)
    {
        this._authServiceWebApiService = authServiceWebApiService;
    }

    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        this.ViewData["ReturnUrl"] = returnUrl;
        return this.View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserModel model, string? returnUrl = null, bool rememberMe = false)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var accessToken = await this._authServiceWebApiService.LoginAsync(model);

        if (accessToken is null)
        {
            this.ModelState.AddModelError("", "Invalid login attempt.");
            return this.View(model);
        }

        return await this.AuthUserAsync(accessToken, model, returnUrl, rememberMe);
    }

    [HttpGet("register")]
    public IActionResult Register(string? returnUrl = null)
    {
        return this.View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserModel model, string? returnUrl = null, bool rememberMe = false)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var authResult = await this._authServiceWebApiService.RegisterAsync(model);

        if (authResult.IsSuccess)
        {
            return await this.AuthUserAsync(authResult.AccessToken!, model, returnUrl, rememberMe);
        }

        if (authResult.Errors != null)
        {
            foreach (var errorEntry in authResult.Errors)
            {
                foreach (var error in errorEntry.Value)
                {
                    this.ModelState.AddModelError(errorEntry.Key, error);
                }
            }
        }
        else
        {
            this.ModelState.AddModelError("", "Під час реєстрації сталася невідома помилка.");
        }

        return this.View(model);

    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return this.RedirectToAction("Index", "Home");
    }

    private async Task<IActionResult> AuthUserAsync(string accessToken, object model, string? returnUrl = null, bool rememberMe = false)
    {
        var user = await this._authServiceWebApiService.AuthMe(accessToken);

        if (user is null)
        {
            this.ModelState.AddModelError("", "Invalid login attempt.");
            return this.View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("access_token", accessToken),
            new Claim("AvatarUrl", user.AvatarUrl ?? DefaultAvatars.DefaultAvatar)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe
        };

        await this.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(returnUrl) && this.Url.IsLocalUrl(returnUrl))
        {
            return this.Redirect(returnUrl);
        }

        return this.RedirectToAction("Index", "Home");
    }
}
