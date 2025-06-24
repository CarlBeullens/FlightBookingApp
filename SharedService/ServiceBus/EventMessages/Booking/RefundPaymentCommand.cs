namespace SharedService.ServiceBus.EventMessages.Booking;

public class RefundPaymentCommand
{
    public Guid BookingId { get; set; }
}