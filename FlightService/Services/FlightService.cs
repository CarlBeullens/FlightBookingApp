using FlightService.Data;
using FlightService.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SharedService.DTOs.Flights;
using SharedService.ServiceBus.EventMessages.Flight;
using SharedService.ServiceBus.Services;

namespace FlightService.Services;

public class FlightService(FlightServiceDbContext context, IMessageService messageService, ILogger<FlightService> logger) : IFlightService
{
    private readonly FlightServiceDbContext _context = context;
    private readonly IMessageService _messageService = messageService;
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

    public async Task<Flight> CancelFlight(Guid id)
    {
        var flight = await _context.Flights.FindAsync(id);

        if (flight == null)
        {
            throw new KeyNotFoundException($"Flight with ID {id} not found.");
        }

        if (flight.FlightStatus == FlightStatus.Cancelled)
        {
            _logger.LogInformation("Flight {FlightFlightNumber} was already cancelled.", flight.FlightNumber);

            return flight;
        }
        
        flight.FlightStatus = FlightStatus.Cancelled;
        await _context.SaveChangesAsync();

        try
        {
            const string queueName = "flight-cancelled";
            
            var payLoad = new FlightCancelledEvent { FlightId = id };
            
            await _messageService.PublishMessageAsync(queueName, payLoad);
        }
            
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while publishing flight cancelled event for flight {Id}", id);
        }

        return flight;
    }

    public async Task DeleteFlightAsync(Guid id)
    {
        var flight = await _context.Flights.FindAsync(id);
        
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<Result<int>> UpdateSeatingAfterConfirmationAsync(Guid id, int seats)
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

            return Result<int>.Failure(result);
        }
        
        var hasCapacity = flight.AvailableSeats >= seats;

        if (!hasCapacity)
        {
            _logger.LogInformation("Not enough seats available on flight {Id}", id);
            
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

            return Result<int>.Failure(result);
        }
            
        var availableSeats = flight.AvailableSeats -= seats;

        await _context.SaveChangesAsync();
            
        return Result<int>.Success(availableSeats);
    }
    
    public async Task<Result<int>> UpdateSeatingAfterCancellationAsync(Guid id, int seats)
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

            return Result<int>.Failure(result);
        }
        
        var availableSeats = flight.AvailableSeats += seats;

        await _context.SaveChangesAsync();
            
        return Result<int>.Success(availableSeats);
    }
}