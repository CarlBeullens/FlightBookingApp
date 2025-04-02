using BookingService.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Extensions;

public static class ResultExtensions
{
    public static ActionResult HandleValidationErrors<T>(this Result<T> result, ControllerBase controller)
    {
        var notFoundError = result.ValidationResult?.Errors
            .FirstOrDefault(e => e.ErrorMessage.Contains("not found"));
        
        if (notFoundError != null)
        {
            var details =  new ProblemDetails
            {
                Title = "Not Found",
                Detail = notFoundError.ErrorMessage,
                Status = StatusCodes.Status404NotFound,
            };

            return controller.NotFound(details);
        }
        
        var notStatusPendingError = result.ValidationResult?.Errors
            .FirstOrDefault(e => e.ErrorMessage.Contains(nameof(BookingStatus.Pending)));

        if (notStatusPendingError != null)
        {
            var details =  new ProblemDetails
            {
                Title = "Validation Failed",
                Detail = notStatusPendingError.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            };
            
            return controller.BadRequest(details);
        }
        
        var notStatusPaidError = result.ValidationResult?.Errors
            .FirstOrDefault(e => e.ErrorMessage.Contains(nameof(PaymentStatus.Paid)));

        if (notStatusPaidError != null)
        {
            var details =  new ProblemDetails
            {
                Title = "Validation Failed",
                Detail = notStatusPaidError.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            };
            
            return controller.BadRequest(details);
        }
        
        var alreadyCancelledError = result.ValidationResult?.Errors
            .FirstOrDefault(e => e.ErrorMessage.Contains(nameof(BookingStatus.Cancelled)));

        if (alreadyCancelledError != null)
        {
            var details =  new ProblemDetails
            {
                Title = "Validation Failed",
                Detail = alreadyCancelledError.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            };

            return controller.BadRequest(details);
        }

        var seatsError = result.ValidationResult?.Errors
            .FirstOrDefault(e => e.ErrorMessage.Contains("seats"));

        if (seatsError != null)
        {
            var details =  new ProblemDetails
            {
                Title = "Insufficient Seat Availability",
                Detail = seatsError.ErrorMessage,
                Status = StatusCodes.Status409Conflict
            };

            return controller.Conflict(details);
        }

        return null!;
    }
    
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        return new ProblemDetails
        {
            Title = "Validation Failed",
            Detail = validationResult.Errors.First().ErrorMessage,
            Status = StatusCodes.Status400BadRequest
        };
    }
}