using System.Globalization;
using BookingService.Extensions;
using BookingService.Mappers;
using BookingService.Models;
using BookingService.Services;
using BookingService.Validators;
using Microsoft.AspNetCore.Mvc;
using SharedService.DTOs.Bookings;
using SharedService.DTOs.Flights;

namespace BookingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController(IBookingService service, ILogger<BookingController> logger) : ControllerBase
{
    private readonly IBookingService _service = service;
    private readonly ILogger<BookingController> _logger = logger;
    
    [HttpGet("public/is-alive", Name = "IsAlive")]
    public ActionResult<string> IsAlive()
    {
        return Ok("Booking Service is alive!");
    }
    
    [HttpGet("private/is-admin", Name = "IsAdmin")]
    public ActionResult<string> IsAdmin()
    {
        var cultureInfo = new CultureInfo("en-GB");
        
        return Ok(new
        {
            message = "Admin access confirmed",
            serverTime = DateTime.Now.ToString(cultureInfo)
        });
    }

    [HttpGet("protected/all-bookings", Name = "GetAllBookings")]
    [ProducesResponseType(typeof(IReadOnlyCollection<Booking>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<Booking>>> GetAllBookingsAsync()
    {
        var bookings = await _service.GetAllBookingsAsync();
        
        return Ok(bookings);
    }
    
    [HttpGet("protected/{id:guid}", Name="GetBookingById")]
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
    
    [HttpGet("protected/reference/{reference}", Name = "GetBookingByReference")]
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
    
    [HttpGet("protected/email/{email}", Name = "GetBookingByEmail")]
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

    [HttpPost("protected/create", Name = "CreateBooking")]
    [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateBookingResponse>> CreateBookingAsync([FromBody] CreateBookingRequest request)
    {
        var validationResult = BookingValidator.ValidateForCreation(request);
        
        if (!validationResult.IsValid)
        {
            var errorMessage = validationResult.Errors.First().ErrorMessage ?? "validation error";
            
            _logger.LogError("Validation failed: {ErrorMessage}", errorMessage);
            
            return this.ValidationErrorProblem(errorMessage);
        }

        var createdBooking = await _service.CreateBookingAsync(request);

        if (createdBooking == null)
        {
            _logger.LogError("Failed to create booking");
            
            return this.ServerErrorProblem("Failed to create booking");
        }

        var response = createdBooking.ToResponse();

        return CreatedAtRoute("GetBookingById", new { id = response.BookingId }, response);
    }
    
    [HttpPatch("protected/confirm/{id:guid}", Name = "ConfirmBooking")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> ConfirmBookingAsync(Guid id)
    {
        var result = await _service.ConfirmBookingAsync(id);

        return result.IsSuccess ? Ok(result.Data) : result.HandleValidationErrors(this);
    }

    [HttpPatch("protected/cancel/{id:guid}", Name = "CancelBooking")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> CancelBookingAsync(Guid id)
    {
        var result = await _service.CancelBookingAsync(id);
        
        return result.IsSuccess ? Ok(result.Data) : result.HandleValidationErrors(this);
    }
    
    [HttpGet("protected/flights/all", Name = "GetAllFlights")]
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
    
    [HttpGet("protected/flights/{id:guid}", Name = "GetFlightDetailsById")]
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