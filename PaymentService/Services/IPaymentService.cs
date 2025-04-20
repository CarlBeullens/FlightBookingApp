using PaymentService.Models;

namespace PaymentService.Services;

public interface IPaymentService
{
    Task<Result<Payment>> CreatePaymentAsync(Guid bookingId);
    
    Task<Result<Payment>> GetPaymentAsync(Guid bookingId);
    
    Task<Result<Payment>> SetPaymentStatus(Guid bookingId, string status);
}