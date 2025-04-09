using FlightService.Models;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace FlightService.Services;

public interface IFlightService
{
    Task<IReadOnlyCollection<Flight>> GetAllFlightsAsync();
    
    Task<Flight?> GetFlightByIdAsync(Guid id);
    
    Task<Flight?> GetFlightByReferenceAsync(string reference);
    
    Task<Flight> CreateFlightAsync(Flight flight);
    
    Task DeleteFlightAsync(Guid id);

    Task<Result<int>> UpdateSeatingAfterConfirmationAsync(Guid id, int seats);
    
    Task<Result<int>> UpdateSeatingAfterCancellationAsync(Guid id, int seats);

    Task<IReadOnlyCollection<Flight>> SearchFlightsAsync(FlightSearchRequest searchRequest);
}