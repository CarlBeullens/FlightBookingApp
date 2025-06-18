using FlightService.Mappers;
using FlightService.Models;
using Microsoft.AspNetCore.Mvc;
using FlightService.Services;
using Microsoft.AspNetCore.Authorization;
using SharedService.DTOs.Flights;

namespace FlightService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightController(IFlightService service) : ControllerBase
{
    private readonly IFlightService _service = service;

    [HttpGet("public/is-alive")]
    public ActionResult<string> IsAlive()
    {
        return Ok("Flight Service is alive!");
    }
    
    [HttpGet("public/search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsResponse>>> SearchFlightsAsync([FromQuery] FlightSearchRequest searchRequest)
    {
        var flights = await _service.SearchFlightsAsync(searchRequest);

        if (flights.Count == 0)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No Flights Found",
                Detail = "No flights match the search criteria.",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        var response = flights.ToDtoReadOnlyCollection();
        
        return Ok(response);
    }
    
    [HttpGet("protected/all-flights")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightDetailsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsResponse>>> GetAllFlightDetailsAsync()
    {
        var flights = await _service.GetAllFlightsAsync();
        
        var response = flights.ToDtoReadOnlyCollection();
        
        return Ok(response);
    }
    
    [HttpGet("protected/{id:guid}")]
    [ProducesResponseType(typeof(FlightDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDetailsResponse?>> GetFlightDetailsByIdAsync(Guid id)
    {
        var flight = await _service.GetFlightByIdAsync(id);
        
        if (flight is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No Flight Found",
                Detail = $"No flight found with {id}",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(flight.ToDto());
    }

    [HttpGet("protected/{reference}")]
    [ProducesResponseType(typeof(FlightDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDetailsResponse?>> GetFlightDetailsByReferenceAsync(string reference)
    {
        var flight = await _service.GetFlightByReferenceAsync(reference);
        
        if (flight is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No Flight Found",
                Detail = $"No flight found with reference {reference}",
                Status = StatusCodes.Status404NotFound
            });
        }
        
        return Ok(flight.ToDto());
    }

    [HttpPatch("protected/cancel/{id:guid}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CancelFlight(Guid id)
    {
        var result = await _service.CancelFlight(id);

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
        }
        
        return Ok($"Flight {id} cancelled successfully");
    }

    [HttpPatch("protected/seats/{id:guid}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Result<bool>>> UpdateAvailableSeats(Guid id, [FromQuery] int seats)
    {
        var result = await _service.UpdateSeatingAfterConfirmationAsync(id, seats);
        
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