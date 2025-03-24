using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Data;

public class FlightServiceDbContext(DbContextOptions<FlightServiceDbContext> options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    //public DbSet<Seat> Seats { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Flight configuration
        modelBuilder.Entity<Flight>().HasKey(f => f.Id);
        modelBuilder.Entity<Flight>().Property(f => f.FlightNumber).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.DepartureCity).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.DepartureLocationCode).HasMaxLength(4).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.ArrivalCity).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.ArrivalLocationCode).HasMaxLength(4).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.Price).HasColumnType("decimal(18,2)");
        
        //Seat configuration
        // modelBuilder.Entity<Seat>().HasKey(s => s.Id);
        // modelBuilder.Entity<Seat>().Property(s => s.SeatNumber).HasMaxLength(10).IsRequired();
        // modelBuilder.Entity<Seat>().Property(s => s.SeatType).HasMaxLength(10).IsRequired();
        // modelBuilder.Entity<Seat>().Property(s => s.IsAvailable).IsRequired();
        
        //Seed data
        modelBuilder.SeedFlights();
    }
}