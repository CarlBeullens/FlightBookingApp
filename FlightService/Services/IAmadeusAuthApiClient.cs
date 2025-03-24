using Refit;
using Shared.DTOs;
using Shared.DTOs.Authorization;

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