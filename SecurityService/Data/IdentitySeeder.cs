using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurityService.Models;

namespace SecurityService.Data;

public static class IdentitySeeder
{
    private static readonly string AdminRoleId = Guid.NewGuid().ToString();
    private static readonly string CustomerRoleId = Guid.NewGuid().ToString();
    private static readonly string AdminUserId = Guid.NewGuid().ToString();
    private static readonly string CustomerUserId = Guid.NewGuid().ToString();
    
    public static void SeedIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole
            {
                Id = CustomerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );

        var customer = new ApplicationUser
        {
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        var admin = new ApplicationUser
        {
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = AdminUserId,
                FirstName = "System",
                LastName = "Administrator",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new ApplicationUser
            {
                Id = CustomerUserId,
                FirstName = "Carl",
                LastName = "Beullens",
                UserName = "carl.beullens@example.com",
                NormalizedUserName = "CARL.BEULLENS@EXAMPLE.COM",
                Email = "carl.beullens@example.com",
                NormalizedEmail = "CARL.BEULLENS@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(customer, "Customer123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = AdminUserId,
                RoleId = AdminRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = CustomerUserId,
                RoleId = CustomerRoleId
            }
        );
    }
}



