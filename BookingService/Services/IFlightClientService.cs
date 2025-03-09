using Refit;
using Shared.DTOs;

namespace BookingService.Services;

public interface IFlightClientService
{ 
    [Get("/api/flight")]
    Task<IReadOnlyCollection<FlightDetailsDto>> GetAllFlightDetailsAsync();

    [Get("/api/flight/{id}")]
    Task<FlightDetailsDto?> GetFlightDetailsByIdAsync(Guid id);
}