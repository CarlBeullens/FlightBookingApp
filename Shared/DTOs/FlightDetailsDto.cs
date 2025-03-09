namespace Shared.DTOs;

public class FlightDetailsDto
{
    public string FlightNumber { get; set; } = string.Empty;
    
    public string DepartureCity { get; set; } = string.Empty;
    
    public string ArrivalCity { get; set; } = string.Empty;
    
    public DateTime DepartureTime { get; set; }
    
    public DateTime ArrivalTime { get; set; }
    
    public int AvailableSeats { get; set; }
    
    public decimal Price { get; set; }
}