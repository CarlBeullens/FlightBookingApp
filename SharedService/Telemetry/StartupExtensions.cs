using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace SharedService.Telemetry;

public static class StartupExtensions
{
    /// <summary>
    /// Configures OpenTelemetry with business metrics and console export for the Flight Booking System.
    /// </summary>
    public static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureAppInsights");
        
        services.AddOpenTelemetry().WithMetrics(options => options
            .AddMeter("FlightBookingService")
            .AddConsoleExporter()
            .AddAzureMonitorMetricExporter(monitorOptions =>
            {
                monitorOptions.ConnectionString = connectionString;
            })
        );
        
        services.AddSingleton<IBusinessMetrics, BusinessMetrics>();
        
        return services;
    }
}