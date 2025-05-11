using Microsoft.AspNetCore.Identity;
using SecurityService.Models;

namespace SecurityService.Services;

public interface IAuthService
{
    Task<IResult> RegisterAsync(UserManager<ApplicationUser> userManager, RegisterRequest request);
    
    Task<IResult> LoginAsync(UserManager<ApplicationUser> userManager, LoginRequest request);
}