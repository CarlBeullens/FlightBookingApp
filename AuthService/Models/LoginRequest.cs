namespace AuthService.Models;

public class LoginRequest
{
    public required Guid Id { get; set; }
    
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}