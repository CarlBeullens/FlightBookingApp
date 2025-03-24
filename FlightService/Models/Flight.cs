namespace FlightService.Models;

public class Flight
{
    public Guid Id { get; set; }
    
    public string FlightNumber { get; set; } = string.Empty;
    
    public string DepartureCity { get; set; } = string.Empty;
    
    public string DepartureLocationCode { get; set; } = string.Empty;
    
    public string ArrivalCity { get; set; } = string.Empty;
    
    public string ArrivalLocationCode { get; set; } = string.Empty;
    
    public DateTime DepartureTime { get; set; }
    
    public DateTime ArrivalTime { get; set; }
    
    public decimal Price { get; set; }
    
    public int AvailableSeats { get; set; }
    
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}