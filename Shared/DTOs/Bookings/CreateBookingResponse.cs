namespace Shared.DTOs.Bookings;

/// <summary>
/// Response model for the create bookings endpoint
/// </summary>
public class CreateBookingResponse
{
    public required Guid BookingId { get; set; }
    
    public required string BookingReference { get; set; }
    
    public required Guid FlightId { get; set; }
    
    public required string FlightNumber { get; set; }
    
    public required string PrimaryContactName { get; set; }
    
    public required string PrimaryContactEmail { get; set; }
    
    public required int NumberOfSeats { get; set; }
    
    public required DateTime BookingDate { get; set; }
    
    public required decimal TotalPrice { get; set; }

    public required string BookingStatus { get; set; }
    
    public required string PaymentStatus { get; set; }
}