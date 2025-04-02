namespace Shared.DTOs.Flights;

/// <summary>
/// Response model for the get flight details endpoint
/// </summary>
public class FlightDetailsResponse
{
    public Guid Id { get; set; }
    
    public required string FlightNumber { get; set; }
    
    public required string DepartureCity { get; set; }
    
    public string DepartureLocationCode { get; set; } = string.Empty;
    
    public required string ArrivalCity { get; set; }
    
    public string ArrivalLocationCode { get; set; } = string.Empty;
    
    public required DateTime DepartureTime { get; set; }
    
    public required DateTime ArrivalTime { get; set; }
    
    public required int AvailableSeats { get; set; }
    
    public required decimal Price { get; set; }
}