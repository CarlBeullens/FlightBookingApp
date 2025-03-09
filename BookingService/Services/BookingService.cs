using BookingService.Data;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BookingService.Services;

public class BookingService(BookingDbContext context, IFlightClientService flightServiceClient, ILogger<BookingService> logger) : IBookingService
{
    private readonly BookingDbContext _context = context;
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
    
    public async Task<IReadOnlyCollection<Booking>> GetBookingsByPassengerEmailAsync(string email)
    {
        return await _context.Bookings.Where(b => b.PassengerEmail == email).ToListAsync();
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        // Get flight details from FlightService
        // does the flight exist?
        // does the flight have enough available seats?
        
        booking.Id = Guid.NewGuid();
        booking.Status = BookingStatus.Pending;
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Booking {BookingId} created", booking.Id);

        return booking;
    }
    
    public async Task<Booking?> ConfirmBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        
        if (booking == null)
        {
            _logger.LogWarning("Cannot confirm booking: Booking {BookingId} not found", id);
            return null;
        }

        if (booking.Status != BookingStatus.Pending)
        {
            return booking;
        }

        booking.Status = BookingStatus.Confirmed;
        await _context.SaveChangesAsync();

        return booking;
    }
    
    public async Task<IReadOnlyCollection<FlightDetailsDto>> GetAllFlightDetailsAsync()
    {
        var flights = await _flightServiceClient.GetAllFlightDetailsAsync();

        return flights ?? new List<FlightDetailsDto>();      
    }
}