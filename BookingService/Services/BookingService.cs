using System.ComponentModel.DataAnnotations;
using BookingService.Data;
using BookingService.Mappers;
using BookingService.Models;
using BookingService.Validators;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Bookings;
using Shared.DTOs.Flights;

namespace BookingService.Services;

public class BookingService(BookingDbContext context, ICacheService cacheService, IFlightClientService flightServiceClient, ILogger<BookingService> logger) 
    : IBookingService
{
    private readonly BookingDbContext _context = context;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IFlightClientService _flightServiceClient = flightServiceClient;
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
        var flightReference = request.FlightId.HasValue ? request.FlightId.ToString() : request.FlightNumber;
        
        if (flightReference == null)
        {
            _logger.LogWarning("Flight reference is null");
            return null;
        }
        
        var flight = await GetFlightDetailsByReferenceAsync(flightReference);

        if (flight == null)
        {
            _logger.LogWarning("The flight with reference {FlightReference} does not exist", flightReference);
            return null;
        }
        
        var booking = BookingMapper.ToDomain(request, flight.Id);
        
        if (flight.AvailableSeats < booking.NumberOfSeats)
        {
            _logger.LogWarning("Not enough available seats on flight {FlightId}", booking.FlightId);
            return null;
        }
        
        var createdBooking = CreateBooking(booking, flight);
        _context.Bookings.Add(createdBooking);
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Booking {BookingId} created", createdBooking.Id);

        return booking;
        
        // after creating the booking, we should update the available seats on the flight but this should happen after the payment and therefore also the booking is confirmed
    }
    
    private Booking CreateBooking(Booking booking, FlightDetailsResponse flight)
    {
        booking.Id = Guid.NewGuid();
        booking.FlightId = flight.Id;
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

        var updateResult = await _flightServiceClient.UpdateAvailableSeatingAsync(booking!.FlightId, booking.NumberOfSeats);

        if (!updateResult.IsSuccess)
        {
            return Result<Booking>.Failure(updateResult.ValidationResult!);
        }
        
        booking.BookingStatus = BookingStatus.Confirmed;
        await _context.SaveChangesAsync();
        
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
    
    public async Task<FlightDetailsResponse?> GetFlightDetailsByReferenceAsync(string reference)
    {
        var cachedKey = $"flight_{reference}";

        var cachedFlight = await _cacheService.GetAsync<FlightDetailsResponse>(cachedKey);
        
        if (cachedFlight != null)
        {
            return cachedFlight;
        }
        
        var flight = await _flightServiceClient.GetFlightDetailsByReferenceAsync(reference);

        if (flight == null)
        {
            _logger.LogWarning("Flight with reference {FlightReference} not found", reference);
            return null;
        }
        
        await _cacheService.SetAsync(cachedKey, flight);
        
        return flight;
    }
}