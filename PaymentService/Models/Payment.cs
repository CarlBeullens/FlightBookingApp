namespace PaymentService.Models;

public class Payment
{
    public Guid Id { get; set; }
    
    public Guid BookingId { get; set; }
    
    public string PaymentStatus { get; set; } = Models.PaymentStatus.None;
    
    public string Currency { get; set; } = "EUR";

    public decimal Amount { get; set; }
    
    public DateTime CreatedAt { get; set; } // creation of the payment object in the db
    
    public DateTime? ProcessedAt { get; set; } // when the payment was processed successfully
}