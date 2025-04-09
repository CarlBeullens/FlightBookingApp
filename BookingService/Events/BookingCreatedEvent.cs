namespace BookingService.Events;

public class BookingCreatedEvent
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
    
    public int NumberOfSeats { get; set; }
}