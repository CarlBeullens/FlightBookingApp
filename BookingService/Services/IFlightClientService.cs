using Refit;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace BookingService.Services;

public interface IFlightClientService
{ 
    [Get("/api/flight")]
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();

    [Get("/api/flight/{id}")]
    Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id);
}