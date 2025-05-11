using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SecurityService.Models;

namespace SecurityService.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<ApplicationUser>().Property(x => x.FirstName).HasMaxLength(50).IsRequired();
        builder.Entity<ApplicationUser>().Property(x => x.LastName).HasMaxLength(50).IsRequired();
        
        // Seeding
        builder.SeedIdentity();
    }
}