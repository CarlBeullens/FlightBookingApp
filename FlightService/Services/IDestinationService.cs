using FlightService.Models;
using Shared.DTOs;
using Shared.DTOs.Activities;
using Shared.DTOs.PointsOfInterest;

namespace FlightService.Services;

public interface IDestinationService
{
    Task<Location> GetLocationDetailsByCodeAsync(string locationCode);
    
    Task<List<PointOfInterest>> GetPointsOfInterestAsync(PointsOfInterestQuery queryParams);
    
    Task<List<Activity>> GetActivitiesAsync(ActivitiesQuery queryParams);
}