using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services;

public class AuthService(ITokenService tokenService) : IAuthService
{
    private readonly ITokenService _tokenService = tokenService;
    
    public async Task<IResult> RegisterAsync(UserManager<ApplicationUser> userManager, RegisterRequest request)
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

    public async Task<IResult> LoginAsync(UserManager<ApplicationUser> userManager, LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return TypedResults.BadRequest("No such user found");
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
                
        if (!isValidPassword)
        {
            return TypedResults.BadRequest("Invalid password");
        }
        
        var token = await _tokenService.CreateJwtToken(user);
                
        return TypedResults.Ok(token);
    }
}