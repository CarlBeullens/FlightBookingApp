using Refit;

namespace SharedService.DTOs.Activities;

/// <summary>
/// Request model for the get activities endpoint
/// </summary>
public class ActivitiesQuery
{
    [AliasAs("latitude")]
    public double Latitude { get; set; }
    
    [AliasAs("longitude")]
    public double Longitude { get; set; }

    [AliasAs("radius")]
    public int Radius { get; set; } = 1;
}