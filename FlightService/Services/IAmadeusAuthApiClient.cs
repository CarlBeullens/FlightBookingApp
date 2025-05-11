using Refit;
using SharedService.DTOs;
using SharedService.DTOs.Auth;

namespace FlightService.Services;

/// <summary>
/// Refit interface for Amadeus authentication
/// </summary>
public interface IAmadeusAuthApiClient
{
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    [Post("/v1/security/oauth2/token")]
    Task<AmadeusTokenResponse> GetTokenAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
}