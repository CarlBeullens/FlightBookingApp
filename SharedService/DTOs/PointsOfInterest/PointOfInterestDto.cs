namespace SharedService.DTOs.PointsOfInterest;

/// <summary>
/// Data transfer object representing a point of interest
/// </summary>
public class PointOfInterestDto
{
    public string Name { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public string Rank { get; set; } = string.Empty;
    
    public decimal Longitude { get; set; }
    
    public decimal Latitude { get; set; }
    
    public List<string> Tags { get; set; } = new();
}