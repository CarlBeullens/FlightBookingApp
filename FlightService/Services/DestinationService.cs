using FlightService.Configuration;
using FlightService.Mappers;
using FlightService.Models;
using Microsoft.Extensions.Options;
using Refit;
using Shared.DTOs.Activities;
using Shared.DTOs.PointsOfInterest;

namespace FlightService.Services;

public class DestinationService(IAmadeusApiClient apiClient, IAmadeusAuthApiClient authApiClient, IOptions<AmadeusOptions> settings, ILogger<DestinationService> logger) : IDestinationService
{
    private readonly IAmadeusApiClient _apiClient = apiClient;
    private readonly IAmadeusAuthApiClient _authApiClient = authApiClient;
    private readonly AmadeusOptions _options = settings.Value;
    private readonly ILogger<DestinationService> _logger = logger;
    private string _accessToken = string.Empty;
    private DateTime _tokenExpiration = new DateTime();
    
    public async Task<Location> GetLocationDetailsByCodeAsync(string locationCode)
    {
        var token = await GetTokenAsync();

        try
        {
            var response = await _apiClient.GetLocationDetailsByCodeAsync($"Bearer {token}", locationCode);
            
            _logger.LogInformation("Received location details for {LocationCode}", locationCode);

            return response.Data.ToModel();
        }
        
        catch (ApiException ex)
        {
            _logger.LogError("API error: {StatusCode} - {ReasonPhrase}", ex.StatusCode, ex.ReasonPhrase);
            throw;
        }
    }

    public async Task<List<PointOfInterest>> GetPointsOfInterestAsync(PointsOfInterestQuery queryParams)
    {
        var token = await GetTokenAsync();
        
        try
        {
            var response = await _apiClient.GetPointsOfInterestAsync($"Bearer {token}", queryParams);
            
            _logger.LogInformation("Received points of interest for location {Latitude}, {Longitude}", queryParams.Latitude, queryParams.Longitude);

            return response.PointsOfInterest.Select(p => p.ToModel()).ToList();
        }
        
        catch (ApiException ex)
        {
            _logger.LogError("Request failed: {Url}", ex.RequestMessage.RequestUri);
            _logger.LogError("API error: {StatusCode} - {ReasonPhrase}", ex.StatusCode, ex.ReasonPhrase);
            throw;
        }
    }
    
    public async Task<List<Activity>> GetActivitiesAsync(ActivitiesQuery queryParams)
    {
        var token = await GetTokenAsync();
        
        try
        {
            var response = await _apiClient.GetActivitiesAsync($"Bearer {token}", queryParams);
            
            _logger.LogInformation("Received activities for location {Latitude}, {Longitude}", queryParams.Latitude, queryParams.Longitude);
            
            return response.Data.Select(a => a.ToModel()).ToList();
        }
        
        catch (ApiException ex)
        {
            _logger.LogError("Request failed: {Url}", ex.RequestMessage.RequestUri);
            _logger.LogError("API error: {StatusCode} - {ReasonPhrase}", ex.StatusCode, ex.ReasonPhrase);
            throw;
        }
    }

    private async Task<string> GetTokenAsync()
    {
        bool expiredToken = DateTime.UtcNow.AddMinutes(5) >= _tokenExpiration;
        
        if (string.IsNullOrEmpty(_accessToken) || expiredToken)
        {
            var request = new Dictionary<string, string>()
            {
                {"grant_type", _options.GrantType},
                {"client_id", _options.ClientId},
                {"client_secret", _options.ClientSecret}
            };
        
            var response = await _authApiClient.GetTokenAsync(request);
            
            _accessToken = response.AccessToken;
            _tokenExpiration = DateTime.UtcNow.AddSeconds(response.ExpiresIn);
        }

        return _accessToken;
    }
}