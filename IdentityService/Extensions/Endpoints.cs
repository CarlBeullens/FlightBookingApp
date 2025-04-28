using System.Security.Claims;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Extensions;

public static class Endpoints
{
    public static void RegisterEndPoints(this WebApplication app)
    {
        app.MapGet("/auth-required", (ClaimsPrincipal user) => $"Hello, {user.Identity?.Name}! You are authenticated.")
            .RequireAuthorization();

        app.MapGet("/admin-only", () => "Admin area")
            .RequireAuthorization("RequireAdminRole");
        
        app.MapPost("/register-extended", async (UserManager<ApplicationUser> userManager, [FromBody] ExtendedRegisterRequest request) =>
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");

                return Results.Ok(new { message = "Registration successful" });
            }

            return Results.BadRequest(result.Errors);
        })
        .AllowAnonymous();
    }
}