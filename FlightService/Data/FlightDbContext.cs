using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Data;

public class FlightDbContext(DbContextOptions<FlightDbContext> options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Flight>().HasKey(f => f.Id);
        modelBuilder.Entity<Flight>().Property(f => f.FlightNumber).HasMaxLength(10).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.DepartureCity).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.ArrivalCity).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Flight>().Property(f => f.Price).HasColumnType("decimal(18,2)");
        
        modelBuilder.SeedFlights();
    }
}