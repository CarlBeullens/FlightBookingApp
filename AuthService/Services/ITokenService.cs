using AuthService.Models;

namespace AuthService.Services;

public interface ITokenService
{
    Task<string> CreateJwtToken(ApplicationUser user);
}