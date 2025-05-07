using Refit;
using SharedService.DTOs.Activities;
using SharedService.DTOs.Locations;
using SharedService.DTOs.PointsOfInterest;

namespace FlightService.Services;

/// <summary>
/// Refit interface for Amadeus services
/// </summary>
public interface IAmadeusApiClient
{
    /// <summary>
    /// Returns specific city details based on its location code
    /// </summary>
    /// <param name="token">OAuth Bearer token</param>
    /// <param name="locationCode">The code of the location</param>
    /// <returns>Detailed information about the location</returns>
    /// <exception cref="ApiException">Thrown when the API request fails</exception>
    [Get("/v1/reference-data/locations/{locationCode}")]
    Task<LocationDetailsResponse> GetLocationDetailsByCodeAsync([Header("Authorization")] string token, string locationCode);
    
    /// <summary>
    /// Retrieves points of interest near a specific geographic location.
    /// </summary>
    /// <param name="token">OAuth Bearer token for authentication (format: "Bearer {token}")</param>
    /// <param name="queryParams">Search parameters including coordinates, radius and filtering options</param>
    /// <returns>A response containing points of interest matching the search criteria</returns>
    /// <remarks>
    /// The queryParams object includes:
    /// - latitude: Decimal geographic coordinate (required)
    /// - longitude: Decimal geographic coordinate (required)
    /// - radius: Search radius in kilometers (0-20, defaults to 1)
    /// - categories: Optional filter by POI categories (comma-separated values: BEACH_PARK, HISTORICAL, NIGHTLIFE, RESTAURANT, SIGHTS, SHOPPING)
    /// - page[limit]: Maximum number of items per page
    /// - page[offset]: Zero-based index of the first item to return
    /// </remarks>
    /// <exception cref="ApiException">Thrown when the API request fails</exception>
    [Get("/v1/reference-data/locations/pois")]
    Task<PointsOfInterestResponse> GetPointsOfInterestAsync([Header("Authorization")] string token, [Query] PointsOfInterestQuery queryParams);
    
    /// <summary>
    /// Retrieves available activities near a specific geographic location.
    /// </summary>
    /// <param name="token">OAuth Bearer token for authentication (format: "Bearer {token}")</param>
    /// <param name="queryParams">Search parameters including coordinates and radius</param>
    /// <returns>A response containing activities matching the search criteria</returns>
    /// <remarks>
    /// The queryParams object includes:
    /// - latitude: Decimal geographic coordinate (required)
    /// - longitude: Decimal geographic coordinate (required)
    /// - radius: Search radius in kilometers (0-20, defaults to 1)
    /// </remarks>
    /// <exception cref="ApiException">Thrown when the API request fails</exception>
    [Get("/v1/shopping/activities")]
    Task<ActivitiesResponse> GetActivitiesAsync([Header("Authorization")] string token, [Query] ActivitiesQuery queryParams);
}