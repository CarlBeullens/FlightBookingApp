using System.Globalization;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data;

public static class BookingSeeder
{
    public static void SeedBookings(this ModelBuilder modelBuilder)
    {
        var cultureInfo = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
        
        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                Id = Guid.Parse("f9f5c229-9508-4d99-9f55-1012bc0f05aa"),
                FlightId = Guid.Parse("0db30750-6cfe-47e2-a453-08ae60570ac6"),
                BookingReference = "BK7439",
                PrimaryContactName = "John Smith",
                PrimaryContactEmail = "john.smith@example.com",
                NumberOfSeats = 2,
                BookingDate = DateTime.SpecifyKind(DateTime.Parse("2025-02-15 14:30:00", cultureInfo), DateTimeKind.Local),
                TotalPrice = 299.98M,
                BookingStatus = BookingStatus.Confirmed
            },
            new Booking
            {
                Id = Guid.Parse("dd64a66a-0f61-40e4-af3e-c11d1218f5ce"),
                FlightId = Guid.Parse("af9eadc1-1cbe-405f-b487-0bf8ccd08164"),
                BookingReference = "BK1258",
                PrimaryContactName = "Emma Johnson",
                PrimaryContactEmail = "emma.johnson@example.com",
                NumberOfSeats = 1,
                BookingDate = DateTime.SpecifyKind(DateTime.Parse("2025-02-17 09:15:00", cultureInfo), DateTimeKind.Local),
                TotalPrice = 179.99M,
                BookingStatus = BookingStatus.Confirmed
            },
            new Booking
            {
                Id = Guid.Parse("c6f7ec6f-6c37-45a6-b0b9-d3f0b8468499"),
                FlightId = Guid.Parse("b25e0c53-526e-4853-899c-1a227313b5fa"),
                BookingReference = "BK3562",
                PrimaryContactName = "Michael Chen",
                PrimaryContactEmail = "michael.chen@example.com",
                NumberOfSeats = 3,
                BookingDate = DateTime.SpecifyKind(DateTime.Parse("2025-02-18 16:45:00", cultureInfo), DateTimeKind.Local),
                TotalPrice = 361.50M,
                BookingStatus = BookingStatus.Pending
            },
            new Booking
            {
                Id = Guid.Parse("a3a96ed2-cb3c-45f0-a18b-15a091123b37"),
                FlightId = Guid.Parse("deaf4a89-8fe7-40ca-9cec-4022d03355e0"),
                BookingReference = "BK9214",
                PrimaryContactName = "Sarah Garcia",
                PrimaryContactEmail = "sarah.garcia@example.com",
                NumberOfSeats = 1,
                BookingDate = DateTime.SpecifyKind(DateTime.Parse("2025-02-20 11:30:00", cultureInfo), DateTimeKind.Local),
                TotalPrice = 149.99M,
                BookingStatus = BookingStatus.Cancelled
            },
            new Booking
            {
                Id = Guid.Parse("a1be6e68-a220-4696-9d71-13c477c647ee"),
                FlightId = Guid.Parse("b24b825c-4dc0-4936-8980-52289ad439c0"),
                BookingReference = "BK7705",
                PrimaryContactName = "David Wilson",
                PrimaryContactEmail = "david.wilson@example.com",
                NumberOfSeats = 2,
                BookingDate = DateTime.SpecifyKind(DateTime.Parse("2025-02-21 08:00:00", cultureInfo), DateTimeKind.Local),
                TotalPrice = 359.98M,
                BookingStatus = BookingStatus.Pending
            }
        );
    }
}