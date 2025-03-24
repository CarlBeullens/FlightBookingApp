namespace FlightService.Models;

public class Activity
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Rating { get; set; } = string.Empty;
    
    public List<string> Pictures { get; set; } = new();
    
    public string BookingLink { get; set; } = string.Empty;
    
    public string PriceAmount { get; set; } = string.Empty;
    
    public string CurrencyCode { get; set; } = string.Empty;
}