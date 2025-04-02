using Shared.DTOs.Passengers;

namespace Shared.DTOs.Bookings;

/// <summary>
/// Request model for the create bookings endpoint
/// </summary>
public class CreateBookingRequest
{
    public Guid? FlightId { get; set; }
    
    public string? FlightNumber { get; set; }
    
    public required string PrimaryContactName { get; set; }
    
    public required string PrimaryContactEmail { get; set; }

    public required List<PassengerDto> Passengers { get; set; } = new List<PassengerDto>();
}