using System.Security.Claims;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Extensions;

public static class Endpoints
{
    public static void AddIdentityEndPoints(this WebApplication app)
    {
        var identity = app.MapGroup("api/identity");
        
        identity.MapPost("register", Register)
            .AllowAnonymous();
        
        identity.MapPost("/login", Login)
            .AllowAnonymous();
        
        identity.MapGet("/auth-required", (ClaimsPrincipal user) => $"Hello, {user.Identity?.Name}! You are authenticated.")
            .RequireAuthorization();

        identity.MapGet("/admin-only", () => "Admin area")
            .RequireAuthorization("RequireAdminRole");
    }

    private static async Task<IResult> Register(UserManager<ApplicationUser> userManager, RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        var registration = await userManager.CreateAsync(user, request.Password);

        if (!registration.Succeeded)
        {
            return Results.BadRequest(registration.Errors);
        }
                    
        await userManager.AddToRoleAsync(user, "Customer");

        return TypedResults.Ok("Registration successful");
    }

    private static async Task<IResult> Login(UserManager<ApplicationUser> userManager, LoginRequest request, ITokenService tokenService)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return TypedResults.NotFound();
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
                
        if (!isValidPassword)
        {
            return TypedResults.Unauthorized();
        }

        var token = tokenService.GenerateJwtToken();
                
        return TypedResults.Ok(token);
    }
}