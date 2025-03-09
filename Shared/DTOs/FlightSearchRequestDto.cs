namespace Shared.DTOs;

public class FlightSearchRequestDto
{
    public string? DepartureCity { get; set; }
    
    public string? ArrivalCity { get; set; }
    
    public DateTime? DepartureTime { get; set; }
    
    public DateTime? ArrivalTime { get; set; } 
    
    public decimal? MaxPrice { get; set; }
}