using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string GenerateJwtToken()
    {
        // Create security key
        var key = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // Create credentials
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Create token
        var expireTimeInMinutes = int.TryParse(config["Jwt:ExpireInMinutes"], out var minutes) ? minutes : 60;
                
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: null,
            expires: DateTime.Now.AddMinutes(expireTimeInMinutes),
            signingCredentials: signingCredentials);
        
        // Return the token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}