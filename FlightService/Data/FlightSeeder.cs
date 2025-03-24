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
                DepartureLocationCode = "CFRA",
                ArrivalCity = "London",
                ArrivalLocationCode = "CLON",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 07:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 08:45:00"), DateTimeKind.Utc),
                Price = 149.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AF220",
                DepartureCity = "Paris",
                DepartureLocationCode = "CPAR",
                ArrivalCity = "Rome",
                ArrivalLocationCode = "CROM",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 09:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 11:00:00"), DateTimeKind.Utc),
                Price = 179.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "IB340",
                DepartureCity = "Madrid",
                DepartureLocationCode = "CMAD",
                ArrivalCity = "Barcelona",
                ArrivalLocationCode = "CBCN",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 10:15:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 11:30:00"), DateTimeKind.Utc),
                Price = 120.50M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "BA110",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Munich",
                ArrivalLocationCode = "CMUC",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 11:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 14:15:00"), DateTimeKind.Utc),
                Price = 162.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "LX225",
                DepartureCity = "Zurich",
                DepartureLocationCode = "CZRH",
                ArrivalCity = "Milan",
                ArrivalLocationCode = "CMIL",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 14:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 15:30:00"), DateTimeKind.Utc),
                Price = 155.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "EK050",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Nice",
                ArrivalLocationCode = "CNCE",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 16:15:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 19:00:00"), DateTimeKind.Utc),
                Price = 179.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "SQ333",
                DepartureCity = "Frankfurt",
                DepartureLocationCode = "CFRA",
                ArrivalCity = "Berlin",
                ArrivalLocationCode = "CBER",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 08:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 09:45:00"), DateTimeKind.Utc),
                Price = 129.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AA190",
                DepartureCity = "Paris",
                DepartureLocationCode = "CPAR",
                ArrivalCity = "New York",
                ArrivalLocationCode = "CNYC",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 10:45:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 13:15:00"), DateTimeKind.Utc),
                Price = 549.99M,
                AvailableSeats = 100
                    
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "BA780",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Madrid",
                ArrivalLocationCode = "CMAD",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 15:30:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 18:45:00"), DateTimeKind.Utc),
                Price = 189.99M,
                AvailableSeats = 100
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AF445",
                DepartureCity = "Rome",
                DepartureLocationCode = "CROM",
                ArrivalCity = "Paris",
                ArrivalLocationCode = "CPAR",
                DepartureTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 18:00:00"), DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse("2025-03-01 20:15:00"), DateTimeKind.Utc),
                Price = 169.99M,
                AvailableSeats = 100
            }
        );
    }
}