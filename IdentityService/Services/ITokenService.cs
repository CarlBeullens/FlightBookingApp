namespace IdentityService.Services;

public interface ITokenService
{
    string GenerateJwtToken();
}