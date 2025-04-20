using FlightService.Models;
using Shared.DTOs.Flights;

namespace FlightService.Mappers;

public static class FlightMapper
{
    public static FlightDetailsResponse ToDto(this Flight model)
    {
        return new FlightDetailsResponse
        {
            Id = model.Id,
            FlightNumber = model.FlightNumber,
            DepartureCity = model.DepartureCity,
            ArrivalCity = model.ArrivalCity,
            DepartureLocationCode = model.DepartureLocationCode,
            ArrivalLocationCode = model.DepartureLocationCode,
            DepartureTime = model.DepartureTime,
            ArrivalTime = model.ArrivalTime,
            Price = model.Price,
            AvailableSeats = model.AvailableSeats,
        };  
    }
    
    public static IReadOnlyCollection<FlightDetailsResponse> ToDtoReadOnlyCollection(this IEnumerable<Flight> models)
    {
        return models.Select(m => m.ToDto()).ToList().AsReadOnly();
    }
}