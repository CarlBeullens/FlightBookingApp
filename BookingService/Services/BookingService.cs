using BookingService.Data;
using BookingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Flights;

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
    
    public async Task<IReadOnlyCollection<Booking>> GetBookingsByEmailAsync(string email)
    {
        return await _context.Bookings.Where(b => b.PassengerEmail == email).ToListAsync();
    }

    public async Task<Booking?> CreateBookingAsync(Booking booking)
    {
        var flight = await GetFlightDetailsByIdAsync(booking.FlightId);

        if (flight == null)
        {
            _logger.LogWarning("The flight with ID {FlightId} does not exist", booking.FlightId);
            return null;
        }
        
        if (flight.AvailableSeats < booking.NumberOfSeats)
        {
            _logger.LogWarning("Not enough available seats on flight {FlightId}", booking.FlightId);
            return null;
        }
        
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
            _logger.LogInformation("Bookings with status {BookingStatus} cannot be confirmed", booking.Status);
            return booking;
        }

        booking.Status = BookingStatus.Confirmed;
        await _context.SaveChangesAsync();

        return booking;
    }

    public async Task<Booking?> CancelBookingAsync(Guid id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        
        if (booking == null)
        {
            _logger.LogWarning("Cannot cancel booking: Booking {BookingId} not found", id);
            return null;
        }

        if (booking.Status != BookingStatus.Cancelled)
        {
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Booking {BookingId} cancelled", id);
        }
        
        return booking;
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
}