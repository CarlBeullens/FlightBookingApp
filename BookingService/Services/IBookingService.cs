using BookingService.Models;
using Shared.DTOs;

namespace BookingService.Services;

public interface IBookingService
{
    Task<IReadOnlyCollection<Booking>> GetAllBookingsAsync();
    
    Task<Booking?> GetBookingByIdAsync(Guid id);
    
    Task<IReadOnlyCollection<Booking>> GetBookingsByPassengerEmailAsync(string email);
    
    Task<Booking> CreateBookingAsync(Booking booking);
    
    Task<IReadOnlyCollection<FlightDetailsDto>> GetAllFlightDetailsAsync();
}