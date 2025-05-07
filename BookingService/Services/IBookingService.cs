using BookingService.Models;
using SharedService.DTOs.Bookings;
using SharedService.DTOs.Flights;

namespace BookingService.Services;

public interface IBookingService
{
    Task<IReadOnlyCollection<Booking>> GetAllBookingsAsync();
    
    Task<Booking?> GetBookingByIdAsync(Guid id);
    
    Task<Booking?> GetBookingByReferenceAsync(string reference);
    
    Task<IReadOnlyCollection<Booking>> GetBookingsByEmailAsync(string email);

    Task<Booking?> CreateBookingAsync(CreateBookingRequest request);

    Task<Result<Booking>> ConfirmBookingAsync(Guid id);
    
    Task<Result<Booking>> CancelBookingAsync(Guid id);
    
    Task<Result<int>> UpdateBookingsAfterCancelledFlightAsync(Guid flightId);
    
    Task<IReadOnlyCollection<FlightDetailsResponse>> GetAllFlightDetailsAsync();
    
    Task<FlightDetailsResponse?> GetFlightDetailsByIdAsync(Guid id);
    
    Task<FlightDetailsResponse?> GetFlightDetailsByReferenceAsync(string flightNumber);
}