namespace BookingService.Models;

public class Booking
{
    public Guid Id { get; set; }
    
    public Guid FlightId { get; set; }
    
    public string BookingReference { get; set; } = string.Empty;
    
    public string PrimaryContactName { get; set; } = string.Empty;
    
    public string PrimaryContactEmail { get; set; } = string.Empty;
    
    public int NumberOfSeats { get; set; }
    
    public DateTime BookingDate { get; set; }
    
    public decimal TotalPrice { get; set; }

    public string BookingStatus { get; set; } = Models.BookingStatus.None;
    
    public string PaymentStatus { get; set; } = Models.PaymentStatus.None;
    
    public ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();
}