namespace SharedService.Messaging.Models.Booking;

public class BookingCancelledEvent
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
    
    public int NumberOfSeats { get; set; }
}