using BookingService.Models;
using Shared.DTOs.Bookings;
using Shared.DTOs.Passengers;

namespace BookingService.Mappers;

public static class BookingMapper
{
    public static Booking ToDomain(CreateBookingRequest request, Guid flightId)
    {
        var passengers = request.Passengers.Select(p => p.ToDomain()).ToList();
        
        return new Booking
        {
            FlightId = flightId,
            PrimaryContactName = request.PrimaryContactName,
            PrimaryContactEmail = request.PrimaryContactEmail,
            NumberOfSeats = passengers.Count,
            Passengers = passengers
        };
    }
    
    public static CreateBookingResponse ToResponse(this Booking booking)
    {
        return new CreateBookingResponse
        {
            BookingId = booking.Id,
            BookingReference = booking.BookingReference,
            FlightId = booking.FlightId,
            FlightNumber = booking.FlightNumber,
            PrimaryContactName = booking.PrimaryContactName,
            PrimaryContactEmail = booking.PrimaryContactEmail,
            NumberOfSeats = booking.NumberOfSeats,
            BookingDate = booking.BookingDate,
            TotalPrice = booking.TotalPrice,
            BookingStatus = booking.BookingStatus
        };
    }

    private static Passenger ToDomain(this PassengerDto dto)
    {
        return new Passenger
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            PassportNumber = dto.PassportNumber,
            Nationality = dto.Nationality
        };
    }
}