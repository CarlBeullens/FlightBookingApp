using BookingService.Models;
using Refit;
using SharedService.DTOs.Flights;

namespace BookingService.Services;

/// <summary>
/// Client interface for communicating with the Flight Service API.
/// </summary>
public interface IFlightClientService

{ 
    [Get("/api/flight/protected/all-flights")]
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();
    
    [Get("/api/flight/protected/{id}")]
    Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id);
    
    [Get("/api/flight/protected/{reference}")]
    Task<FlightDetailsResponse?> GetFlightDetailsByReferenceAsync(string reference);

    [Patch("/api/flight/protected/seats/{id}")]
    Task<Result<bool>> UpdateAvailableSeatingAsync(Guid id, int seatsToReserve);
}