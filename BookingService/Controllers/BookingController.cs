using BookingService.Models;
using BookingService.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Bookings;
using Shared.DTOs.Flights;

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

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<Booking>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<Booking>>> GetAllBookingsAsync()
    {
        var bookings = await _service.GetAllBookingsAsync();
        
        return Ok(bookings);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> GetBookingByIdAsync(Guid id)
    {
        var booking = await _service.GetBookingByIdAsync(id);
        
        if (booking is null)
        {
            return NotFound();
        }
        
        return Ok(booking);
    }
    
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<Booking>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<Booking>>> GetBookingsByEmailAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email cannot be empty");
        }
        
        var bookings = await _service.GetBookingsByEmailAsync(email);
        
        return Ok(bookings);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateBookingResponse>> CreateBookingAsync([FromBody] CreateBookingRequest request)
    {
        // add validation on the request
        
        var booking = request.Adapt<Booking>();
        
        var createdBooking = await _service.CreateBookingAsync(booking);

        if (createdBooking == null)
        {
            _logger.LogWarning("Failed to create booking");
            return StatusCode(500, "Failed to create booking");
        }

        var response = createdBooking.Adapt<CreateBookingResponse>();
        
        return CreatedAtAction(nameof(GetBookingByIdAsync), new { id = response.Id }, response);
    }
    
    [HttpPatch("confirm/{id:guid}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> ConfirmBookingAsync(Guid id)
    {
        var booking = await _service.ConfirmBookingAsync(id);
        
        if (booking == null)
        {
            return NotFound($"Booking with ID {id} not found");
        }

        return Ok(booking);
    }

    [HttpPatch("cancel/{id:guid}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> CancelBookingAsync(Guid id)
    {
        var booking = await _service.CancelBookingAsync(id);
        
        if (booking == null)
        {
            return NotFound($"Booking with ID {id} not found");
        }

        return Ok(booking);
    }
    
    [HttpGet("flights")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FlightDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyCollection<FlightDetailsResponse>>> GetAllFlightDetailsAsync()
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
    
    [HttpGet("flights/{id:guid}")]
    [ProducesResponseType(typeof(FlightDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FlightDetailsResponse>> GetFlightDetailsByIdAsync(Guid id)
    {
        try
        {
            var flightDetails = await _service.GetFlightDetailsByIdAsync(id);
            
            if (flightDetails == null)
            {
                return NotFound();
            }
            
            _logger.LogInformation("Successfully retrieved flight details for ID {FlightId}", id);
            return Ok(flightDetails);
        }
        
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving flight details for ID {FlightId}", id);
            return StatusCode(500, "Failed to retrieve flight details");
        }
    }
}