namespace Shared.DTOs.Payments;

/// <summary>
/// Data transfer object representing a payment
/// </summary>
public class PaymentDto
{
    public Guid Id { get; set; }
    
    public required string PaymentStatus { get; set; }

    public decimal Amount { get; set; }
    
    public DateTime? PaymentProcessedAt { get; set; }
}