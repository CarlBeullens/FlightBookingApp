using BookingService.Models;
using Shared.DTOs.Flights;

namespace BookingService.Services;

public interface IBookingService
{
    Task<IReadOnlyCollection<Booking>> GetAllBookingsAsync();
    
    Task<Booking?> GetBookingByIdAsync(Guid id);
    
    Task<IReadOnlyCollection<Booking>> GetBookingsByEmailAsync(string email);
    
    Task<Booking?> CreateBookingAsync(Booking booking);

    Task<Booking?> ConfirmBookingAsync(Guid id);
    
    Task<Booking?> CancelBookingAsync(Guid id);
    
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();
    
    Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id);
}