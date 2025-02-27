using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);
        modelBuilder.Entity<Booking>().Property(b => b.FlightId).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.PassengerName).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.PassengerEmail).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.BookingDate).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.NumberOfSeats).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Booking>().Property(b => b.Status).IsRequired();
    }
}