using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SharedService.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError("An exception occurred: {ExceptionMessage}", exception.Message);

        var problemDetails = CreateProblemDetails(exception);
        
        httpContext.Response.StatusCode = problemDetails.Status.GetValueOrDefault();
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        return exception switch
        {
            KeyNotFoundException => new ProblemDetails
            {
                Title = "Not Found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            },
            
            _ => new  ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            }
        };
    }
}