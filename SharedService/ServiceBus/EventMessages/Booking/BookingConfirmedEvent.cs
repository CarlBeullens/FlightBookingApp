namespace SharedService.ServiceBus.EventMessages.Booking;

public class BookingConfirmedEvent
{
    public Guid BookingId { get; set; }
    
    public Guid FlightId { get; set; }
    
    public int NumberOfSeats { get; set; }
}