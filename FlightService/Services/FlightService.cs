using FlightService.Data;
using FlightService.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace FlightService.Services;

public class FlightService(FlightServiceDbContext context, ILogger<FlightService> logger) : IFlightService
{
    private readonly FlightServiceDbContext _context = context;
    private readonly ILogger<FlightService> _logger = logger;

    public async Task<IReadOnlyCollection<Flight>> SearchFlightsAsync(FlightSearchRequest searchRequest)
    {
        var query = _context.Flights.AsQueryable();
        
        query = !string.IsNullOrWhiteSpace(searchRequest.DepartureCity) 
            ? query.Where(f => f.DepartureCity.Equals(searchRequest.DepartureCity))
            : query;
        
        query = !string.IsNullOrWhiteSpace(searchRequest.ArrivalCity) 
            ? query.Where(f => f.ArrivalCity.Equals(searchRequest.ArrivalCity))
            : query;
        
        query = searchRequest.MaxPrice.HasValue 
            ? query.Where(f => f.Price <= searchRequest.MaxPrice) 
            : query;

        query = searchRequest.DepartureTime.HasValue
            ? query.Where(f => f.DepartureTime >= searchRequest.DepartureTime)
            : query;
        
        query = searchRequest.ArrivalTime.HasValue
            ? query.Where(f => f.ArrivalTime <= searchRequest.ArrivalTime)
            : query;

        return await query.ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<Flight>> GetAllFlightsAsync()
    {
        return await _context.Flights.ToListAsync();
    }
    
    public async Task<Flight?> GetFlightByIdAsync(Guid id)
    {
        return await _context.Flights.FindAsync(id);
    }
    
    public async Task<Flight?> GetFlightByReferenceAsync(string reference)
    {
        if (Guid.TryParse(reference, out var id))
        {
            return await GetFlightByIdAsync(id);
        }

        return await _context.Flights.SingleOrDefaultAsync(f => f.FlightNumber == reference);
    }

    public async Task<Flight> CreateFlightAsync(Flight flight)
    {
        flight.Id = Guid.NewGuid();
        
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        return flight;
    }

    public async Task DeleteFlightAsync(Guid id)
    {
        var flight = await _context.Flights.FindAsync(id);
        
        if (flight is not null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<Result<bool>> UpdateAvailableSeatingAsync(Guid id, int seatsToReserve)
    {
        var flight = await GetFlightByIdAsync(id);

        if (flight == null)
        {
            var result = new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        ErrorMessage = $"Flight {id} not found"
                    }
                }
            };

            return Result<bool>.Failure(result);
        }

        var hasCapacity = flight.AvailableSeats >= seatsToReserve;

        if (!hasCapacity)
        {
            _logger.LogInformation("Not enough seats available on flight {id}", id);
            
            var result = new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure
                    {
                        ErrorMessage = $"Not enough seats available on flight {id}"
                    }
                }
            };

            return Result<bool>.Failure(result);
        }
        
        flight.AvailableSeats -= seatsToReserve;
        await _context.SaveChangesAsync();
            
        return Result<bool>.Success(true);
    }
}