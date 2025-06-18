using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Booking configuration
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);
        modelBuilder.Entity<Booking>().Property(b => b.FlightId).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.FlightNumber).HasMaxLength(5).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.BookingReference).HasMaxLength(6).IsRequired(); // PNR - Passenger Name Record codes are usually 6 characters long
        modelBuilder.Entity<Booking>().Property(b => b.PrimaryContactName).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.PrimaryContactEmail).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.BookingDate).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.NumberOfSeats).IsRequired();
        modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Booking>().Property(b => b.BookingStatus).HasMaxLength(10).IsRequired();
        
        // Passenger configuration
        modelBuilder.Entity<Passenger>().HasKey(p => p.Id);
        modelBuilder.Entity<Passenger>().Property(p => p.BookingId).IsRequired();
        modelBuilder.Entity<Passenger>().Property(p => p.SeatId).IsRequired(false);
        modelBuilder.Entity<Passenger>().Property(p => p.FirstName).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Passenger>().Property(p => p.LastName).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Passenger>().Property(p => p.DateOfBirth).IsRequired();
        modelBuilder.Entity<Passenger>().Property(p => p.PassportNumber).HasMaxLength(20).IsRequired();
        modelBuilder.Entity<Passenger>().Property(p => p.Nationality).HasMaxLength(20).IsRequired();
        
        // Indexes
        modelBuilder.Entity<Booking>().HasIndex(b => b.BookingReference).IsUnique();
        
        // Relationships
        modelBuilder.Entity<Booking>()
            .HasMany(b => b.Passengers)
            .WithOne(p => p.Booking).HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Seeding
        modelBuilder.SeedPassengers();
        modelBuilder.SeedBookings();
    }
}