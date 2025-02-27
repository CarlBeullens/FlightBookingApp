using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Data;

public static class FlightSeeder
{
    public static void SeedFlights(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>().HasData(
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "LH450",
                DepartureCity = "Frankfurt",
                ArrivalCity = "London",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 07:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 08:45:00"), DateTimeKind.Utc),
                Price = 149.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AF220",
                DepartureCity = "Paris",
                ArrivalCity = "Rome",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 09:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 11:00:00"), DateTimeKind.Utc),
                Price = 179.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "IB340",
                DepartureCity = "Madrid",
                ArrivalCity = "Amsterdam",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 10:15:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 12:45:00"), DateTimeKind.Utc),
                Price = 165.50M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "SK110",
                DepartureCity = "Stockholm",
                ArrivalCity = "Berlin",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 11:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 13:15:00"), DateTimeKind.Utc),
                Price = 142.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "LX225",
                DepartureCity = "Zurich",
                ArrivalCity = "Vienna",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 14:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 15:30:00"), DateTimeKind.Utc),
                Price = 155.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "EK050",
                DepartureCity = "London",
                ArrivalCity = "Dubai",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 22:15:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-02 08:45:00"), DateTimeKind.Utc),
                Price = 459.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "SQ333",
                DepartureCity = "Frankfurt",
                ArrivalCity = "Singapore",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 21:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-02 16:45:00"), DateTimeKind.Utc),
                Price = 689.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AA190",
                DepartureCity = "Paris",
                ArrivalCity = "New York",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 10:45:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 13:15:00"), DateTimeKind.Utc),
                Price = 549.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "TK780",
                DepartureCity = "Amsterdam",
                ArrivalCity = "Istanbul",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 15:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 20:00:00"), DateTimeKind.Utc),
                Price = 229.99M
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "QR445",
                DepartureCity = "Rome",
                ArrivalCity = "Doha",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 23:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-02 06:30:00"), DateTimeKind.Utc),
                Price = 419.99M
            }
        );
    }
}