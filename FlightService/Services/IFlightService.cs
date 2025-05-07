using FlightService.Models;
using SharedService.DTOs;
using SharedService.DTOs.Flights;

namespace FlightService.Services;

public interface IFlightService
{
    Task<IReadOnlyCollection<Flight>> GetAllFlightsAsync();
    
    Task<Flight?> GetFlightByIdAsync(Guid id);
    
    Task<Flight?> GetFlightByReferenceAsync(string reference);
    
    Task<Flight> CreateFlightAsync(Flight flight);

    public Task<Result<Flight>> CancelFlight(Guid id);
    
    Task DeleteFlightAsync(Guid id);

    Task<Result<int>> UpdateSeatingAfterConfirmationAsync(Guid id, int seats);
    
    Task<Result<int>> UpdateSeatingAfterCancellationAsync(Guid id, int seats);

    Task<IReadOnlyCollection<Flight>> SearchFlightsAsync(FlightSearchRequest searchRequest);
}