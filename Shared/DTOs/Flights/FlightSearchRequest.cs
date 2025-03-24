namespace Shared.DTOs.Flights;

/// <summary>
/// Request model for the get flight details endpoint
/// </summary>
public class FlightSearchRequest
{
    public string? DepartureCity { get; set; }
    
    public string? ArrivalCity { get; set; }
    
    public DateTime? DepartureTime { get; set; }
    
    public DateTime? ArrivalTime { get; set; } 
    
    public decimal? MaxPrice { get; set; }
}