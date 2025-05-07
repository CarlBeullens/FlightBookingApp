namespace SharedService.DTOs.Passengers;

/// <summary>
/// Data transfer object representing a passenger
/// </summary>
public class PassengerDto
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required DateOnly DateOfBirth { get; set; }
    
    public required string PassportNumber { get; set; }
    
    public required string Nationality { get; set; }
}