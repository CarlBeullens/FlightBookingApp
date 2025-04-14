using System.Globalization;
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
                DepartureTime = TimeOnly.ParseExact("07:30", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("08:45", "HH:mm", CultureInfo.InvariantCulture),
                Price = 149.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AF220",
                DepartureCity = "Paris",
                DepartureLocationCode = "CPAR",
                ArrivalCity = "Rome",
                ArrivalLocationCode = "CROM",
                DepartureTime = TimeOnly.ParseExact("09:00", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("11:00", "HH:mm", CultureInfo.InvariantCulture),
                Price = 179.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "IB340",
                DepartureCity = "Madrid",
                DepartureLocationCode = "CMAD",
                ArrivalCity = "Barcelona",
                ArrivalLocationCode = "CBCN",
                DepartureTime = TimeOnly.ParseExact("10:15", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("11:30", "HH:mm", CultureInfo.InvariantCulture),
                Price = 120.50M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "BA110",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Munich",
                ArrivalLocationCode = "CMUC",
                DepartureTime = TimeOnly.ParseExact("11:30", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("14:15", "HH:mm", CultureInfo.InvariantCulture),
                Price = 162.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "LX225",
                DepartureCity = "Zurich",
                DepartureLocationCode = "CZRH",
                ArrivalCity = "Milan",
                ArrivalLocationCode = "CMIL",
                DepartureTime = TimeOnly.ParseExact("14:00", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("15:30", "HH:mm", CultureInfo.InvariantCulture),
                Price = 155.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "EK050",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Nice",
                ArrivalLocationCode = "CNCE",
                DepartureTime = TimeOnly.ParseExact("16:00", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("19:00", "HH:mm", CultureInfo.InvariantCulture),
                Price = 179.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "SQ333",
                DepartureCity = "Frankfurt",
                DepartureLocationCode = "CFRA",
                ArrivalCity = "Berlin",
                ArrivalLocationCode = "CBER",
                DepartureTime = TimeOnly.ParseExact("08:30", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("09:45", "HH:mm", CultureInfo.InvariantCulture),
                Price = 129.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AA190",
                DepartureCity = "Paris",
                DepartureLocationCode = "CPAR",
                ArrivalCity = "New York",
                ArrivalLocationCode = "CNYC",
                DepartureTime = TimeOnly.ParseExact("10:45", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("13:15", "HH:mm", CultureInfo.InvariantCulture),
                Price = 549.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
                    
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "BA780",
                DepartureCity = "London",
                DepartureLocationCode = "CLON",
                ArrivalCity = "Madrid",
                ArrivalLocationCode = "CMAD",
                DepartureTime = TimeOnly.ParseExact("15:30", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("18:45", "HH:mm", CultureInfo.InvariantCulture),
                Price = 189.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            },
            new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = "AF445",
                DepartureCity = "Rome",
                DepartureLocationCode = "CROM",
                ArrivalCity = "Paris",
                ArrivalLocationCode = "CPAR",
                DepartureTime = TimeOnly.ParseExact("18:00", "HH:mm", CultureInfo.InvariantCulture),
                ArrivalTime = TimeOnly.ParseExact("20:15", "HH:mm", CultureInfo.InvariantCulture),
                Price = 169.99M,
                AvailableSeats = 100,
                FlightStatus = FlightStatus.Scheduled
            }
        );
    }
}