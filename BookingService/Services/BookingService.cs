using BookingService.Data;
using BookingService.Events;
using BookingService.Mappers;
using BookingService.Models;
using BookingService.Validators;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Bookings;
using Shared.DTOs.Flights;
using Shared.Messaging.Services;

namespace BookingService.Services;

public class BookingService(BookingDbContext context, IFlightClientService flightServiceClient, ICacheService cacheService, IMessageService messageService, ILogger<BookingService> logger) 
    : IBookingService
{
    private readonly BookingDbContext _context = context;
    private readonly IFlightClientService _flightServiceClient = flightServiceClient;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IMessageService _messageService = messageService;
    private readonly ILogger<BookingService> _logger = logger;
    
    public async Task<IReadOnlyCollection<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.ToListAsync();
    }
    
    public async Task<Booking?> GetBookingByIdAsync(Guid id)
    {
        return await _context.Bookings.FindAsync(id);
    }
    
    public async Task<Booking?> GetBookingByReferenceAsync(string reference)
    {
        return await _context.Bookings.SingleOrDefaultAsync(b => b.BookingReference == reference);
    }
    
    public async Task<IReadOnlyCollection<Booking>> GetBookingsByEmailAsync(string email)
    {
        return await _context.Bookings.Where(b => b.PrimaryContactEmail == email).ToListAsync();
    }

    public async Task<Booking?> CreateBookingAsync(CreateBookingRequest request)
    {
        if (string.IsNullOrEmpty(request.FlightNumber))
        {
            _logger.LogWarning("Flight number is not provided");
            return null;
        }
        
        var flight = await GetFlightDetailsByReferenceAsync(request.FlightNumber);

        if (flight == null)
        {
            _logger.LogWarning("The flight with number {FlightNumber} does not exist", request.FlightNumber);
            return null;
        }
        
        var booking = BookingMapper.ToDomain(request, flight.Id);
        
        if (flight.AvailableSeats < booking.NumberOfSeats)
        {
            _logger.LogWarning("Not enough available seats on flight {FlightNumber}", booking.FlightNumber);
            return null;
        }
        
        var createdBooking = CreateBooking(booking, flight);
        _context.Bookings.Add(createdBooking);
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Booking {BookingId} created", createdBooking.Id);

        return booking;
    }
    
    private Booking CreateBooking(Booking booking, FlightDetailsResponse flight)
    {
        booking.Id = Guid.NewGuid();
        booking.FlightId = flight.Id;
        booking.FlightNumber = flight.FlightNumber;
        booking.BookingReference = GenerateBookingReference();
        booking.BookingStatus = BookingStatus.Pending;
        booking.PaymentStatus = PaymentStatus.Pending;
        booking.TotalPrice = flight.Price * booking.NumberOfSeats;
        booking.BookingDate = DateTime.UtcNow;
        
        return booking;
    }

    private string GenerateBookingReference()
    {
        const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    
        var random = Random.Shared;
        var chars = new char[6];
    
        for (int i = 0; i < 6; i++)
        {
            chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
        }
    
        return new string(chars);
    }

    public async Task<Result<Booking>> ConfirmBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        // Payment status = Paid to be validated. Right now it is commented.
        
        var result = BookingValidator.ValidateForConfirmation(booking);
        
        if (!result.IsValid)
        {
            result.Errors.ForEach(e => _logger.LogError("{errorMessage}", e.ErrorMessage));

            return Result<Booking>.Failure(result);
        }
        
        booking!.BookingStatus = BookingStatus.Confirmed;
        await _context.SaveChangesAsync();
        
        try
        {
            var queueName = "booking-confirmed";
            
            var payload = new BookingConfirmedEvent
            {
                BookingId = booking.Id,
                FlightId = booking.FlightId,
                NumberOfSeats = booking.NumberOfSeats
            };
            
            await _messageService.PublishMessageAsync(queueName, payload);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish booking confirmed message for booking {BookingId}", booking.Id);
        }
        
        var cacheKey = $"flight_{booking.FlightNumber}";
        await _cacheService.RemoveAsync(cacheKey);
        
        return Result<Booking>.Success(booking);
    }

    public async Task<Result<Booking>> CancelBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        
        var result = BookingValidator.ValidateForCancellation(booking);

        if (!result.IsValid)
        {
            result.Errors.ForEach(e => _logger.LogError("{errorMessage}", e.ErrorMessage));

            return Result<Booking>.Failure(result);
        }
        
        booking!.BookingStatus = BookingStatus.Cancelled;
        await _context.SaveChangesAsync();
        
        //this needs to trigger a message to the payment service to return the money

        try
        {
            var queueName = "booking-cancelled";
            
            var payload = new BookingCancelledEvent
            {
                BookingId = booking.Id,
                FlightId = booking.FlightId,
                NumberOfSeats = booking.NumberOfSeats
            };
            
            await _messageService.PublishMessageAsync(queueName, payload);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish booking cancelled message for booking {BookingId}", booking.Id);
        }
            
        _logger.LogInformation("Booking {BookingId} cancelled", id);
            
        return Result<Booking>.Success(booking);
    }
    
    public async Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync()
    {
        var flights = await _flightServiceClient.GetAllFlightDetailsAsync();

        return flights ?? new List<FlightDetailsResponse>();      
    }

    public async Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id)
    {
        var flight = await _flightServiceClient.GetFlightDetailsByIdAsync(id);

        if (flight == null)
        {
            _logger.LogWarning("Flight with ID {FlightId} not found", id);
            return null;
        }
        
        return flight;
    }
    
    public async Task<FlightDetailsResponse?> GetFlightDetailsByReferenceAsync(string flightNumber)
    {
        var cacheKey = $"flight_{flightNumber}";

        var cachedFlight = await _cacheService.GetAsync<FlightDetailsResponse>(cacheKey);
        
        if (cachedFlight != null)
        {
            return cachedFlight;
        }
        
        var flight = await _flightServiceClient.GetFlightDetailsByReferenceAsync(flightNumber);

        if (flight == null)
        {
            _logger.LogWarning("Flight with number {FlightReference} not found", flightNumber);
            return null;
        }
        
        await _cacheService.SetAsync(cacheKey, flight);
        
        return flight;
    }
}