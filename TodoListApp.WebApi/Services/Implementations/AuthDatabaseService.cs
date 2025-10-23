using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Data.Entities;
using TodoListApp.WebApi.Exceptions;
using TodoListApp.WebApi.Mappers;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.UserDtos;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class AuthDatabaseService : IAuthDatabaseService
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthDatabaseService(UserManager<User> userManager, ApplicationDbContext context, IConfiguration configuration)
    {
        this._userManager = userManager;
        this._context = context;
        this._configuration = configuration;
    }

    public async Task<string?> Register(RegisterUserDto userDto)
    {
        await using var transaction = await this._context.Database.BeginTransactionAsync();

        var user = new User
        {
            UserName = userDto.UserName,
            AvatarUrl = userDto.AvatarUrl,
            Email = userDto.Email
        };

        IdentityResult createResult = await this._userManager.CreateAsync(user, userDto.Password);
        if(!createResult.Succeeded)
        {
            throw new AuthInvalidException(createResult.Errors);
        }

        IdentityResult roleResult = await this._userManager.AddToRoleAsync(user, Roles.Member);
        if(!roleResult.Succeeded)
        {
            throw new AuthInvalidException(createResult.Errors);
        }

        await transaction.CommitAsync();

        return await this.Login(UserMappers.LoginUserDto(userDto));
    }

    public async Task<string?> Login(LoginUserDto userDto)
    {
        var user = await this._userManager.FindByEmailAsync(userDto.Email);
        if (user is null || !await this._userManager.CheckPasswordAsync(user, userDto.Password))
        {
            return null;
        }

        var roles = await this._userManager.GetRolesAsync(user);

        // Secret Key, Issuer, Audience, Expiration
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Jwt:SecretKey"]!));

        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(this._configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = this._configuration["Jwt:Issuer"],
            Audience = this._configuration["Jwt:Audience"],
        };

        var tokenHandler = new JsonWebTokenHandler();

        string accessToken = tokenHandler.CreateToken(tokenDescriptor);

        return accessToken;
    }

    public async Task<User?> GetCurrentUser(string userId)
    {
        var user = await this._userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return null;
        }

        return user;
    }
}
