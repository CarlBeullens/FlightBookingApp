using Microsoft.AspNetCore.Mvc;
using FlightService.Services;
using Mapster;
using Shared.DTOs;
using Shared.DTOs.Flights;

namespace FlightService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightController(IFlightService service) : ControllerBase
{
    private readonly IFlightService _service = service;

    [HttpGet("isAlive")]
    public ActionResult<string> IsAlive()
    {
        return Ok("Flight Service is alive!");
    }
    
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightDetailsResponse>), StatusCodes.Status200OK)]
    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsResponse>>> SearchFlightsAsync([FromQuery] FlightSearchRequest searchRequest)
    {
        var flights = await _service.SearchFlightsAsync(searchRequest);
        
        return Ok(flights.Adapt<IReadOnlyCollection<FlightDetailsResponse>>());
    }
    
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightDetailsResponse>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsResponse>>> GetAllFlightDetailsAsync()
    {
        var flights = await _service.GetAllFlightsAsync();
        
        return Ok(flights.Adapt<IReadOnlyCollection<FlightDetailsResponse>>());
    }
    
    [ProducesResponseType(typeof(FlightDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightDetailsResponse?>> GetFlightDetailsByIdAsync(Guid id)
    {
        var flight = await _service.GetFlightByIdAsync(id);
        
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(flight.Adapt<FlightDetailsResponse>());
    }
}