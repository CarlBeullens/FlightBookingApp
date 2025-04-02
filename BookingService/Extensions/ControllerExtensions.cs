using Microsoft.AspNetCore.Mvc;

namespace BookingService.Extensions;

public static class ControllerExtensions
{
    public static ActionResult ValidationErrorProblem(this ControllerBase controller, string detail)
    {
        return controller.BadRequest(new ProblemDetails
        {
            Title = "Validation Failed",
            Detail = detail,
            Status = StatusCodes.Status400BadRequest
        });
    }
    
    public static ActionResult ServerErrorProblem(this ControllerBase controller, string detail = "An unexpected error occurred")
    {
        return controller.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
        {
            Title = "Internal Server Error",
            Detail = detail,
            Status = StatusCodes.Status500InternalServerError
        });
    }
}