namespace FlightService.Messages;

public class FlightUpdatedMessage
{
    public Guid FlightId { get; set; }
    
    public int AvailableSeats { get; set; }
    
    public decimal Price { get; set; }
}