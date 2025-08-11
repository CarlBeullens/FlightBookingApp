using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace SharedService.Logging;

public static class StartupExtensions
{
    public static void AddStructuredLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        
        var connectionString = builder.Configuration.GetConnectionString("AzureAppInsights");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("AzureAppInsights connection string is required but not found in configuration.");
        }
        
        builder.Host.UseSerilog((context, config) =>
        {
            var serviceName = context.HostingEnvironment.ApplicationName.ToLower(new CultureInfo("en-GB"));
            var fileSizeLimitBytes = builder.Configuration.GetValue<int>("Logging:FileSizeLimitMegaBytes:Default") * 1024 * 1024;
            
            config
                .MinimumLevel.Debug()
                
                .MinimumLevel.Override("Azure.Messaging.ServiceBus", LogEventLevel.Warning) 
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
                
                .WriteTo.File(
                    path: $"Logs/{serviceName}.txt", 
                    rollingInterval: RollingInterval.Infinite, 
                    fileSizeLimitBytes: fileSizeLimitBytes,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj} {Properties}{NewLine}{Exception}")
                
                .WriteTo.ApplicationInsights(connectionString, TelemetryConverter.Traces, LogEventLevel.Fatal)
                
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
        });
    }
}