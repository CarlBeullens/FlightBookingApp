namespace BookingService.Models;

public class BookingCreatedMessage
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
    
    public int NumberOfSeats { get; set; }
}