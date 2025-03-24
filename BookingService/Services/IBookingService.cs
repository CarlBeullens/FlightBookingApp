using BookingService.Models;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace BookingService.Services;

public interface IBookingService
{
    Task<IReadOnlyCollection<Booking>> GetAllBookingsAsync();
    
    Task<Booking?> GetBookingByIdAsync(Guid id);
    
    Task<IReadOnlyCollection<Booking>> GetBookingsByPassengerEmailAsync(string email);
    
    Task<Booking> CreateBookingAsync(Booking booking);
    
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();
}