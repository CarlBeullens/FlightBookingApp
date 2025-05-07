using FlightService.Models;
using SharedService.DTOs;
using SharedService.DTOs.Activities;
using SharedService.DTOs.PointsOfInterest;

namespace FlightService.Services;

public interface IDestinationService
{
    Task<Location> GetLocationDetailsByCodeAsync(string locationCode);
    
    Task<List<PointOfInterest>> GetPointsOfInterestAsync(PointsOfInterestQuery queryParams);
    
    Task<List<Activity>> GetActivitiesAsync(ActivitiesQuery queryParams);
}