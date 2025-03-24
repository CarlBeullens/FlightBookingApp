using FlightService.Models;
using Refit;
using Shared.DTOs;
using Shared.DTOs.Activities;
using Shared.DTOs.Locations;
using Shared.DTOs.PointsOfInterest;

namespace FlightService.Services;

public interface IAmadeusApiClient
{
    /// <summary>
    /// Returns specific city details based on its location code
    /// </summary>
    /// <param name="token">OAuth Bearer token</param>
    /// <param name="locationCode">The code of the location</param>
    /// <returns>Detailed information about the location</returns>
    [Get("/v1/reference-data/locations/{locationCode}")]
    Task<LocationDetailsResponse> GetLocationDetailsByCodeAsync([Header("Authorization")] string token, string locationCode);
    
    /// <summary>
    /// Returns points of interest for a given location and radius
    /// </summary>
    /// <param name="token">OAuth Bearer token</param>
    /// <param name="queryParams">Object containing the below described parameters</param>
    /// <param name="latitude">Latitude (decimal coordinates)</param>
    /// <param name="longitude">Longitude (decimal coordinates)</param>
    /// <param name="radius">Radius of the search in kilometer. Can be from 0 to 20, default value is 1 Km.</param>
    /// <param name="categories">The category of the location. Multiple value can be selected using a comma i.e. BEACH_PARK, HISTORICAL, NIGHTLIFE, RESTAURANT, SIGHTS, SHOPPING</param>
    /// <param name="page[limit]">Maximum items in one page</param>
    /// <param name="page[offset]">Start index of the requested page</param>
    /// <returns>Points of interest for a given location and radius</returns>
    [Get("/v1/reference-data/locations/pois")]
    Task<PointsOfInterestResponse> GetPointsOfInterestAsync([Header("Authorization")] string token, [Query] PointsOfInterestQuery queryParams);
    
    /// <summary>
    /// Returns activities for a given location and radius
    /// </summary>
    /// <param name="token">OAuth Bearer token</param>
    /// <param name="queryParams">Object containing the below described parameters</param>
    /// <param name="latitude">Latitude (decimal coordinates)</param>
    /// <param name="longitude">Longitude (decimal coordinates)</param>
    /// <param name="radius">Radius of the search in kilometer. Can be from 0 to 20, default value is 1 Km.</param>
    /// <returns>Returns activities for a given location and radius</returns>
    [Get("/v1/shopping/activities")]
    Task<ActivitiesResponse> GetActivitiesAsync([Header("Authorization")] string token, [Query] ActivitiesQuery queryParams);
    
}