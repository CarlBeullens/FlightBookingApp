namespace FlightService.Models;

public class Location
{
    public string CityName { get; set; } = string.Empty;
    
    public string CountryName { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public string TimeZoneOffset { get; set; } = string.Empty;
}