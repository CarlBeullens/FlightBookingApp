using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace BookingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IBookingService service, ILogger<BookingController> logger) : ControllerBase
{
    private readonly IBookingService _service = service;
    private readonly ILogger<BookingController> _logger = logger;
    
    [HttpGet("isAlive")]
    public ActionResult<string> IsAlive()
    {
        return Ok("Booking Service is alive!");
    }
    
    [HttpGet("flights")]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetails>>> GetAllFlightDetailsAsync()
    {
        try
        {
            var flightDetails = await _service.GetAllFlightDetailsAsync();
            _logger.LogInformation("Successfully retrieved all flight details");
            return Ok(flightDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving flight details");
            return StatusCode(500, "Failed to retrieve flight details");
        }
    }
}