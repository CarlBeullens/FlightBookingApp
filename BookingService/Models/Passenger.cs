namespace BookingService.Models;

public class Passenger
{
    public Guid Id { get; set; }
    
    public Guid BookingId { get; set; }
    
    public Guid? SeatId { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateOnly DateOfBirth { get; set; }
    
    public string PassportNumber { get; set; } = string.Empty;
    
    public string Nationality { get; set; } = string.Empty;

    public Booking? Booking { get; set; }
}