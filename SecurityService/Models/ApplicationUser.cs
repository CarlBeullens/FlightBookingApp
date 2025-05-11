using Microsoft.AspNetCore.Identity;

namespace SecurityService.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public override string Email { get; set; } = string.Empty;
}