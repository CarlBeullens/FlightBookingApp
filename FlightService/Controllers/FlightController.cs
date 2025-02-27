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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetails>>> GetAllFlightDetailsAsync()
    {
        var flights = await _service.GetAllFlightsAsync();
        
        return Ok(flights.Adapt<IReadOnlyCollection<FlightDetails>>());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightDetails?>> GetFlightDetailsByIdAsync(Guid id)
    {
        var flight = await _service.GetFlightByIdAsync(id);
        
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(flight.Adapt<FlightDetails>());
    }
}