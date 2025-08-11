using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SharedService.Security;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration config, ILogger<ApiKeyMiddleware> logger)
{
    private readonly string _apiKey = config.GetValue<string>("ApiKey") 
                                      ?? throw new InvalidOperationException("API Key not found");
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger") || 
            context.Request.Path.StartsWithSegments("/health"))
        {
            await next(context);

            return;
        }
        
        _ = context.Request.Headers.TryGetValue("X-API-Key", out var providedApiKey);
        
        if (_apiKey != providedApiKey)
        {
            logger.LogWarning("API key invalid in request to {Path}", context.Request.Path);
            
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Unauthorized",
                message = "API key is required"
            });
            
            return;
        }
        
        await next(context);
    }
}