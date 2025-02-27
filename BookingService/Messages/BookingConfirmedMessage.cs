namespace BookingService.Messages;

public class BookingConfirmedMessage
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
}