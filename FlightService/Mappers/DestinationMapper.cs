using FlightService.Models;
using Shared.DTOs;
using Shared.DTOs.Activities;
using Shared.DTOs.Locations;

namespace FlightService.Mappers;

public static class DestinationMapper
{
    public static Activity ToModel(this ActivityData dto)
    {
        return new Activity
        {
            Name = dto.Name,
            Description = dto.Description,
            Rating = dto.Rating,
            Pictures = dto.Pictures,
            BookingLink = dto.BookingLink,
            PriceAmount = dto.Price.Amount,
            CurrencyCode = dto.Price.CurrencyCode
        };
    }

    public static Location ToModel(this LocationData dto)
    {
        return new Location
        {
            CityName = dto.Address.CityName,
            CountryName = dto.Address.CountryName,
            Latitude = dto.GeoCode.Latitude,
            Longitude = dto.GeoCode.Longitude,
            TimeZoneOffset = dto.TimeZoneOffset
        };
    }
}