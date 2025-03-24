using System.Text.Json.Serialization;

namespace Shared.DTOs.Authorization;

/// <summary>
/// Response model for the Amadeus OAuth token endpoint
/// </summary>
public class AmadeusTokenResponse
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;
    
    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;
    
    [JsonPropertyName("application_name")]
    public string ApplicationName { get; init; } = string.Empty;
    
    [JsonPropertyName("client_id")]
    public string ClientId { get; init; } = string.Empty;
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; } = string.Empty;
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }
    
    [JsonPropertyName("state")]
    public string State { get; init; } = string.Empty;
    
    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;
}