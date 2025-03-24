namespace FlightService.Models;

public class Seat
{
    public Guid Id { get; set; }
    
    public string SeatNumber { get; set; } = string.Empty;

    public string SeatType { get; set; } = string.Empty;
    
    public bool IsAvailable { get; set; } = true;
}