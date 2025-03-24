using FlightService.Data;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace FlightService.Services;

public class FlightService(FlightServiceDbContext context) : IFlightService
{
    private readonly FlightServiceDbContext _context = context;
    
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
}