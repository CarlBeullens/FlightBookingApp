using FlightService.Data;
using FlightService.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Services;

public class FlightService(FlightDbContext context) : IFlightService
{
    private readonly FlightDbContext _context = context;
    
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