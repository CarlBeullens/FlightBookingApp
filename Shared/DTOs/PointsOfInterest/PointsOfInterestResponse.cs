namespace Shared.DTOs.PointsOfInterest;

/// <summary>
/// Response model for the get points of interest endpoint
/// </summary>
public class PointsOfInterestResponse
{
    public List<PointOfInterestDto> PointsOfInterest { get; set; } = new();
}