using Microsoft.AspNetCore.Identity;
using SecurityService.Models;
using SecurityService.Services;
using SharedService.DTOs.Auth;

namespace SecurityService.Extensions;

public static class Endpoints
{
    public static void AddAuthEndPoints(this WebApplication app)
    {
        var auth = app.MapGroup("api/auth")
            .WithTags("Authentication")  
            .WithOpenApi();

        auth.MapPost("register", RegisterAsync)
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .WithName("Register");

        auth.MapPost("/login", LoginAsync)
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .WithName("Login");
    }

    private static Task<IResult> RegisterAsync(IAuthService authService, UserManager<ApplicationUser> userManager, RegisterRequestDto dto)
    {
        var request = new RegisterRequest
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password,
            EmailConfirmed = false
        };
        
        return authService.RegisterAsync(userManager, request);
    }
    
    private static Task<IResult> LoginAsync(IAuthService authService, UserManager<ApplicationUser> userManager, LoginRequestDto dto)
    {
        var request = new LoginRequest
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Password = dto.Password
        };
        
        return authService.LoginAsync(userManager, request);
    }
}