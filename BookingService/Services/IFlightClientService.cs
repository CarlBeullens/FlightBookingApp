using BookingService.Models;
using Refit;
using Shared.DTOs.Flights;

namespace BookingService.Services;

/// <summary>
/// Client interface for communicating with the Flight Service API.
/// </summary>
public interface IFlightClientService

{ 
    [Get("/api/flight")]
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();
    
    [Get("/api/flight/{id}")]
    Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id);
    
    [Get("/api/flight/{reference}")]
    Task<FlightDetailsResponse?> GetFlightDetailsByReferenceAsync(string reference);

    [Patch("/api/flight/seats/{id}")]
    Task<Result<bool>> UpdateAvailableSeatingAsync(Guid id, int seatsToReserve);
}