using Microsoft.AspNetCore.Mvc;
using PaymentService.Services;
using SharedService.DTOs.Payments;
using SharedService.Enums;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService service) : ControllerBase
{
    private readonly IPaymentService _service = service;
    
    [HttpGet("protected/{bookingId:guid}", Name = "GetPaymentForBooking")]
    [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> GetPayment(Guid bookingId)
    {
        var payment = await _service.GetPaymentAsync(bookingId);

        if (!payment.IsSuccess)
        {
            var details = new ProblemDetails
            {
                Title = "No result found",
                Detail = payment.ValidationResult!.Errors.First().ErrorMessage,
                Status = StatusCodes.Status404NotFound
            };
            
            return NotFound(details);
        }

        var result = new PaymentDto
        {
            Id = payment.Data!.Id,
            PaymentStatus = payment.Data.PaymentStatus,
            Amount = payment.Data.Amount,
            PaymentProcessedAt = payment.Data.ProcessedAt
        };
        
        return Ok(result);
    }

    [HttpPut("protected/status", Name = "SetPaymentStatus")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> SetPaymentStatus([FromQuery] Guid bookingId, [FromQuery] PaymentStatus status)
    {
        var payment = await _service.SetPaymentStatus(bookingId, status.ToString());

        if (!payment.IsSuccess)
        {
            var details = new ProblemDetails
            {
                Title = "No result found",
                Detail = payment.ValidationResult!.Errors.First().ErrorMessage,
                Status = StatusCodes.Status404NotFound
            };
            
            return NotFound(details);
        }

        var result = new PaymentDto
        {
            Id = payment.Data!.Id,
            PaymentStatus = payment.Data.PaymentStatus,
            Amount = payment.Data.Amount,
            PaymentProcessedAt = payment.Data.ProcessedAt
        };
        
        return Ok(result);
    }
}