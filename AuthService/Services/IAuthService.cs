using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services;

public interface IAuthService
{
    Task<IResult> RegisterAsync(UserManager<ApplicationUser> userManager, RegisterRequest request);
    
    Task<IResult> LoginAsync(UserManager<ApplicationUser> userManager, LoginRequest request);
}