using Refit;

namespace Shared.DTOs.PointsOfInterest;

/// <summary>
/// Request model for the get points of interest endpoint
/// </summary>
public class PointsOfInterestQuery
{
    [AliasAs("latitude")]
    public double Latitude { get; set; }
    
    [AliasAs("longitude")]
    public double Longitude { get; set; }

    [AliasAs("radius")]
    public int Radius { get; set; } = 1;
    
    [AliasAs("categories")]
    public string Categories { get; set; } = string.Empty;

    [AliasAs("page[limit]")]
    public int PageLimit { get; set; } = 10;

    [AliasAs("page[offset]")]
    public int PageOffset { get; set; } = 0;
}