using BookingService.Extensions;
using BookingService.Mappers;
using BookingService.Models;
using BookingService.Services;
using BookingService.Validators;
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
    
    [HttpGet("{id:guid}", Name="GetBookingById")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> GetBookingByIdAsync(Guid id)
    {
        var booking = await _service.GetBookingByIdAsync(id);
        
        return booking == null 
            ? NotFound(new ProblemDetails
            {
                Title = "No result found",
                Detail = $"Booking with id {id} not found",
                Status = StatusCodes.Status404NotFound
            }) 
            : Ok(booking);
    }
    
    [HttpGet("reference/{reference}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<Booking>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Booking>> GetBookingByReferenceAsync(string reference)
    {
        var validationResult = BookingValidator.ValidateBookingReference(reference);
        
        if (!validationResult.IsValid)
        {
            var details = new ProblemDetails
            {
                Title = "Validation Failed",
                Detail = "Booking reference (PNR) must be 6 characters long",
                Status = StatusCodes.Status400BadRequest
            };
            
            return BadRequest(details);
        }
        
        var booking = await _service.GetBookingByReferenceAsync(reference);
        
        return Ok(booking);
    }
    
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<Booking>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyCollection<Booking>>> GetBookingsByEmailAsync(string email)
    {
        var validationResult = BookingValidator.ValidateEmail(email);

        if (!validationResult.IsValid)
        {
            var details = new ProblemDetails
            {
                Title = "Validation Failed",
                Detail = "Email format was invalid",
                Status = StatusCodes.Status400BadRequest
            };
            
            return BadRequest(details);
        }
        
        var bookings = await _service.GetBookingsByEmailAsync(email);
        
        return Ok(bookings);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateBookingResponse>> CreateBookingAsync([FromBody] CreateBookingRequest request)
    {
        var validationResult = BookingValidator.ValidateForCreation(request);
        
        if (!validationResult.IsValid)
        {
            var errorMessage = validationResult.Errors.First().ErrorMessage ?? "validation error";
            
            _logger.LogError("Validation failed: {detail}", errorMessage);
            
            return this.ValidationErrorProblem(errorMessage);
        }

        var createdBooking = await _service.CreateBookingAsync(request);

        if (createdBooking == null)
        {
            var errorMessage = "Failed to create booking";
            
            _logger.LogError(errorMessage);
            
            return this.ServerErrorProblem(errorMessage);
        }

        var response = createdBooking.ToResponse();

        return CreatedAtRoute("GetBookingById", new { id = response.BookingId }, response);
    }
    
    [HttpPatch("confirm/{id:guid}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> ConfirmBookingAsync(Guid id)
    {
        var result = await _service.ConfirmBookingAsync(id);

        return result.IsSuccess ? Ok(result.Data) : result.HandleValidationErrors(this);
    }

    [HttpPatch("cancel/{id:guid}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> CancelBookingAsync(Guid id)
    {
        var result = await _service.CancelBookingAsync(id);
        
        return result.IsSuccess ? Ok(result.Data) : result.HandleValidationErrors(this);
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