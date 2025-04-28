namespace FlightService.Configuration;

public class AmadeusSettings
{
    public const string Token = "AmadeusToken";
    
    public string GrantType { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;
    
    public string ClientSecret { get; set; } = string.Empty;
}