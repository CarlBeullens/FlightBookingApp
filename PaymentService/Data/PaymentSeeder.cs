using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data;

public static class PaymentSeeder
{
    public static void SeedPayments(this ModelBuilder modelBuilder)
    {
        var cultureInfo = new CultureInfo("en-GB");
        
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = Guid.Parse("f9f5c229-9508-4d99-9f55-1012bc0f05aa"),
                PaymentStatus = PaymentStatus.Paid,
                Amount = 299.98M,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-15 14:35:00", cultureInfo), DateTimeKind.Local),
                ProcessedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-15 14:40:00", cultureInfo), DateTimeKind.Local)
            },
            
            new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = Guid.Parse("dd64a66a-0f61-40e4-af3e-c11d1218f5ce"),
                PaymentStatus = PaymentStatus.Paid,
                Amount = 179.99M,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-17 09:20:00", cultureInfo), DateTimeKind.Local),
                ProcessedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-17 09:25:00", cultureInfo), DateTimeKind.Local)
            },
            
            new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = Guid.Parse("c6f7ec6f-6c37-45a6-b0b9-d3f0b8468499"),
                PaymentStatus = PaymentStatus.Pending,
                Amount = 361.50M,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-18 16:50:00", cultureInfo), DateTimeKind.Local),
                ProcessedAt = null
            },
            
            new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = Guid.Parse("a3a96ed2-cb3c-45f0-a18b-15a091123b37"),
                PaymentStatus = PaymentStatus.Refunded,
                Amount = 149.99M,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-20 11:35:00", cultureInfo), DateTimeKind.Local),
                ProcessedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-20 14:25:00", cultureInfo), DateTimeKind.Local)
            },
            
            new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = Guid.Parse("a1be6e68-a220-4696-9d71-13c477c647ee"),
                PaymentStatus = PaymentStatus.Pending,
                Amount = 359.98M,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2025-02-21 08:05:00", cultureInfo), DateTimeKind.Local),
                ProcessedAt = null
            }
        );
    }
}