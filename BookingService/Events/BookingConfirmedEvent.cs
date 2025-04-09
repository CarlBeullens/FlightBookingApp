namespace BookingService.Events;

public class BookingConfirmedEvent
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
    
    public int NumberOfSeats { get; set; }
}