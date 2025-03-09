using Microsoft.AspNetCore.Mvc;
using FlightService.Services;
using Mapster;
using Shared.DTOs;

namespace FlightService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightController(IFlightService service) : ControllerBase
{
    private readonly IFlightService _service = service;

    [HttpGet("search")]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsDto>>> SearchFlightsAsync([FromQuery] FlightSearchRequestDto searchRequest)
    {
        var flights = await _service.SearchFlightsAsync(searchRequest);
        
        return Ok(flights.Adapt<IReadOnlyCollection<FlightDetailsDto>>());
    }
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsDto>>> GetAllFlightDetailsAsync()
    {
        var flights = await _service.GetAllFlightsAsync();
        
        return Ok(flights.Adapt<IReadOnlyCollection<FlightDetailsDto>>());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightDetailsDto?>> GetFlightDetailsByIdAsync(Guid id)
    {
        var flight = await _service.GetFlightByIdAsync(id);
        
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(flight.Adapt<FlightDetailsDto>());
    }
}