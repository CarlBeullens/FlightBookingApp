namespace Shared.Messaging.Models.Booking;

public class RefundPaymentCommand
{
    public Guid BookingId { get; set; }
}