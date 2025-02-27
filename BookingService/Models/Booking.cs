namespace BookingService.Models;

public class Booking
{
    public Guid Id { get; set; }
    
    public Guid FlightId { get; set; }
    
    public string PassengerName { get; set; } = string.Empty;
    
    public string PassengerEmail { get; set; } = string.Empty;
    
    public int NumberOfSeats { get; set; }
    
    public DateTime BookingDate { get; set; }
    
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.None;
}