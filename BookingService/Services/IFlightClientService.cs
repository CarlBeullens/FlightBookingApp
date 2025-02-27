using Refit;
using Shared.DTOs;

namespace BookingService.Services;

public interface IFlightClientService
{ 
    [Get("/api/flight")]
    Task<IReadOnlyCollection<FlightDetails>> GetAllFlightDetailsAsync();

    [Get("/api/flight/{id}")]
    Task<FlightDetails?> GetFlightDetailsByIdAsync(Guid id);
}