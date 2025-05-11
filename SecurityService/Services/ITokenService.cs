using SecurityService.Models;

namespace SecurityService.Services;

public interface ITokenService
{
    Task<string> CreateJwtToken(ApplicationUser user);
}