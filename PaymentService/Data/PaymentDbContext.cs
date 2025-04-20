using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Payment>().HasKey(p => p.Id);
        modelBuilder.Entity<Payment>().Property(p => p.BookingId).IsRequired();
        modelBuilder.Entity<Payment>().Property(p => p.PaymentStatus).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Payment>().Property(p => p.Currency).HasMaxLength(3).IsRequired();
        modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Payment>().Property(p => p.ProcessedAt).IsRequired(false);
        
        modelBuilder.SeedPayments();
    }
}