using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Services;

public class PaymentService(PaymentDbContext context) : IPaymentService
{
    private readonly PaymentDbContext _context = context;

    public async Task<Result<Payment>> GetPaymentAsync(Guid bookingId)
    {
        var payment = await _context.Payments.SingleOrDefaultAsync(p => p.BookingId == bookingId);

        if (payment == null)
        {
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        ErrorMessage = $"Payment for booking {bookingId} not found"
                    }
                }
            };
            
            return Result<Payment>.Failure(validationResult);
        }
        
        var result = new Result<Payment>
        {
            Data = payment
        };
        
        return result;
    }

    public async Task<Result<Payment>> SetPaymentStatus(Guid bookingId, string status)
    {
        var payment = await GetPaymentAsync(bookingId);
        
        if (!payment.IsSuccess)
        {
            return Result<Payment>.Failure(payment.ValidationResult!);
        }

        var invalidStatus = status != PaymentStatus.Paid && status != PaymentStatus.Pending && status != PaymentStatus.Refunded;

        if (invalidStatus)
        {
            var validationResult = new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        ErrorMessage = "Invalid payment status"
                    }
                }
            };
                
            return Result<Payment>.Failure(validationResult);
        }
            
        payment.Data!.PaymentStatus = status;
        payment.Data!.ProcessedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new Result<Payment> { Data = payment.Data };
    }

    public async Task<Result<Payment>> CreatePaymentAsync(Guid bookingId)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            PaymentStatus = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Payments.Add(payment);
        
        await _context.SaveChangesAsync();

        return new Result<Payment> { Data = payment };
    }
}