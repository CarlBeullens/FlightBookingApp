using FlightService.Models;
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

    [ProducesResponseType(typeof(FlightDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{reference}")]
    public async Task<ActionResult<FlightDetailsResponse?>> GetFlightDetailsByIdAsync(string reference)
    {
        var flight = await _service.GetFlightByReferenceAsync(reference);
        
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(flight.Adapt<FlightDetailsResponse>());
    }

    [HttpPatch("seats/{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Result<bool>>> UpdateAvailableSeats(Guid id, [FromQuery] int seatsToReserve)
    {
        var result = await _service.UpdateAvailableSeatingAsync(id, seatsToReserve);
        
        if (!result.IsSuccess)
        {
            var notFoundError = result.ValidationResult?.Errors
                .FirstOrDefault(e => e.ErrorMessage.Contains("not found"));
                
            if (notFoundError != null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Flight Not Found",
                    Detail = notFoundError.ErrorMessage,
                    Status = StatusCodes.Status404NotFound
                });
            }
            
            var seatsError = result.ValidationResult?.Errors
                .FirstOrDefault(e => e.ErrorMessage.Contains("seats"));
                
            if (seatsError != null)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Insufficient Seat Availability",
                    Detail = seatsError.ErrorMessage,
                    Status = StatusCodes.Status409Conflict
                });
            }
        }
        
        return Ok(result);
    }
}