namespace FlightService.Messages;

public class FlightCreatedMessage
{
    public Guid FlightId { get; set; }
    
    public string FlightNumber { get; set; } = string.Empty;
    
    public DateTime DepartureTime { get; set; }
    
    public int AvailableSeats { get; set; }
    
    public decimal Price { get; set; }
}