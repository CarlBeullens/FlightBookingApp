namespace SharedService.DTOs.Activities;

/// <summary>
/// Response model for the get activities endpoint
/// </summary>
public class ActivitiesResponse
{
    public List<ActivityData> Data { get; set; } = new();
}

public class ActivityData
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Rating { get; set; } = string.Empty;
    
    public List<string> Pictures { get; set; } = new();
    
    public string BookingLink { get; set; } = string.Empty;
    
    public ElementaryPrice Price { get; set; } = new();
}

public class ElementaryPrice
{
    public string Amount { get; set; } = string.Empty;
    
    public string CurrencyCode { get; set; } = string.Empty;
}