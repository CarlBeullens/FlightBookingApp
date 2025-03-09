using FlightService.Models;
using Shared.DTOs;

namespace FlightService.Services;

public interface IFlightService
{
    Task<IReadOnlyCollection<Flight>> GetAllFlightsAsync();
    
    Task<Flight?> GetFlightByIdAsync(Guid id);
    
    Task<Flight> CreateFlightAsync(Flight flight);
    
    Task DeleteFlightAsync(Guid id);

    Task<IReadOnlyCollection<Flight>> SearchFlightsAsync(FlightSearchRequestDto searchRequest);
}