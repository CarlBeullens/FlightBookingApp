using SharedService.DTOs.Passengers;

namespace SharedService.DTOs.Bookings;

/// <summary>
/// Request model for the create bookings endpoint
/// </summary>
public class CreateBookingRequest
{
    public required string FlightNumber { get; set; }
    
    public required string PrimaryContactName { get; set; }
    
    public required string PrimaryContactEmail { get; set; }

    public required List<PassengerDto> Passengers { get; set; } = new List<PassengerDto>();
}