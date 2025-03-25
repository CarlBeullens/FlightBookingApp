namespace Shared.DTOs.Bookings;

/// <summary>
/// Request model for the create bookings endpoint
/// </summary>
public class CreateBookingRequest
{
    public Guid FlightId { get; set; }
    
    public string PassengerName { get; set; } = string.Empty;
    
    public string PassengerEmail { get; set; } = string.Empty;
    
    public int NumberOfSeats { get; set; }
    
    public DateTime BookingDate { get; set; }
    
    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = string.Empty;
}