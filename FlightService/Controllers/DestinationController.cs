using FlightService.Models;
using FlightService.Services;
using Microsoft.AspNetCore.Mvc;
using SharedService.DTOs;
using SharedService.DTOs.Activities;
using SharedService.DTOs.PointsOfInterest;

namespace FlightService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationController(IDestinationService destinationService) : ControllerBase
{
    private readonly IDestinationService _destinationService = destinationService;
    
    [HttpGet("{locationCode}")]
    public async Task<ActionResult<Location>> GetLocationDetailsByCodeAsync(string locationCode)
    {
        var location = await _destinationService.GetLocationDetailsByCodeAsync(locationCode);
        
        return Ok(location);
    }
    
    [HttpGet("pois")]
    public async Task<ActionResult<List<PointOfInterest>>> GetPointsOfInterestAsync([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] int radius = 1)
    {
        var pointsOfInterest = await _destinationService.GetPointsOfInterestAsync(new PointsOfInterestQuery
        {
            Latitude = latitude,
            Longitude = longitude,
            Radius = radius
        });
    
        return Ok(pointsOfInterest);
    }
    
    [HttpGet("activities")]
    public async Task<ActionResult<List<Activity>>> GetActivitiesAsync([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] int radius = 1)
    {
        var activities = await _destinationService.GetActivitiesAsync(new ActivitiesQuery
        {
            Latitude = latitude,
            Longitude = longitude,
            Radius = radius
        });
    
        return Ok(activities);
    }
}