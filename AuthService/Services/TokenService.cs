using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class TokenService(UserManager<ApplicationUser> userManager, IConfiguration config) : ITokenService
{
    public async Task<string> CreateJwtToken(ApplicationUser user)
    {
        // Create claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new (ClaimTypes.Name, user.FirstName + " " + user.LastName),
        };
        
        var roles = await userManager.GetRolesAsync(user);
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        // Create signing key
        var secretKey = config.GetValue<string>("JwtCreation:SecretKey") 
                                ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // Create credentials
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Create token
        var expireTimeInMinutes = int.TryParse(config["JwtCreation:ExpireInMinutes"], out var minutes) ? minutes : 60;
                
        var tokenDescriptor = new JwtSecurityToken(
            issuer: config.GetValue<string>("JwtCreation:Issuer"),
            audience: config.GetValue<string>("JwtCreation:Audience"),
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireTimeInMinutes),
            signingCredentials: signingCredentials);
        
        // Return the token
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}