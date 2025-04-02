using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public static class PassengerSeeder
{
    public static void SeedPasssengers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Passenger>().HasData(
            // Passengers for John Smith's booking
            new Passenger
            {
                Id = Guid.Parse("f7b69891-512a-4a51-b1f1-a4ead0d4ef2a"),
                BookingId = Guid.Parse("f9f5c229-9508-4d99-9f55-1012bc0f05aa"),
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 6, 15),
                PassportNumber = "AB123456",
                Nationality = "United States"
            },
            new Passenger
            {
                Id = Guid.Parse("1fbe9f45-2508-48f9-bde9-beef62dc8a33"),
                BookingId = Guid.Parse("f9f5c229-9508-4d99-9f55-1012bc0f05aa"),
                FirstName = "Mary",
                LastName = "Smith",
                DateOfBirth = new DateTime(1987, 3, 22),
                PassportNumber = "AB789012",
                Nationality = "United States"
            },
            
            // Passenger for Emma Johnson's booking
            new Passenger
            {
                Id = Guid.Parse("2c89026c-c3a1-4fd4-9d5a-d6071c1efaa2"),
                BookingId = Guid.Parse("dd64a66a-0f61-40e4-af3e-c11d1218f5ce"),
                FirstName = "Emma",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1990, 11, 8),
                PassportNumber = "CD345678",
                Nationality = "Canada"
            },
            
            // Passengers for Michael Chen's booking
            new Passenger
            {
                Id = Guid.Parse("3a0d61ef-e10e-4866-ac2b-c2cfd5bde9d3"),
                BookingId = Guid.Parse("c6f7ec6f-6c37-45a6-b0b9-d3f0b8468499"),
                FirstName = "Michael",
                LastName = "Chen",
                DateOfBirth = new DateTime(1980, 4, 12),
                PassportNumber = "EF901234",
                Nationality = "China"
            },
            new Passenger
            {
                Id = Guid.Parse("4b6d3c2a-1e58-456b-bad8-7dcf95f22e8f"),
                BookingId = Guid.Parse("c6f7ec6f-6c37-45a6-b0b9-d3f0b8468499"),
                FirstName = "Lin",
                LastName = "Chen",
                DateOfBirth = new DateTime(1982, 7, 3),
                PassportNumber = "EF567890",
                Nationality = "China"
            },
            new Passenger
            {
                Id = Guid.Parse("5c7e4d3b-2f69-467c-cbe9-8edf06f33e90"),
                BookingId = Guid.Parse("c6f7ec6f-6c37-45a6-b0b9-d3f0b8468499"),
                FirstName = "Jason",
                LastName = "Chen",
                DateOfBirth = new DateTime(2010, 2, 25),
                PassportNumber = "EF123456",
                Nationality = "China"
            },
            
            // Passenger for Sarah Garcia's booking
            new Passenger
            {
                Id = Guid.Parse("6d8e5f4c-3a7a-478d-dcf0-9fe017f44fa1"),
                BookingId = Guid.Parse("a3a96ed2-cb3c-45f0-a18b-15a091123b37"),
                FirstName = "Sarah",
                LastName = "Garcia",
                DateOfBirth = new DateTime(1992, 9, 18),
                PassportNumber = "GH789012",
                Nationality = "Mexico"
            },
            
            // Passengers for David Wilson's booking
            new Passenger
            {
                Id = Guid.Parse("7e9f605d-4a8b-489e-edf1-0afb28a55ab2"),
                BookingId = Guid.Parse("a1be6e68-a220-4696-9d71-13c477c647ee"),
                FirstName = "David",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1975, 5, 27),
                PassportNumber = "IJ345678",
                Nationality = "United Kingdom"
            },
            new Passenger
            {
                Id = Guid.Parse("8f0a7b6e-5c9c-490f-fea2-1aac39b66ac3"),
                BookingId = Guid.Parse("a1be6e68-a220-4696-9d71-13c477c647ee"),
                FirstName = "Elizabeth",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1978, 12, 4),
                PassportNumber = "IJ901234",
                Nationality = "United Kingdom"
            }
        );
    }
}