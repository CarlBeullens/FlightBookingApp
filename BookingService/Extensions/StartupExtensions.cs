using Azure.Messaging.ServiceBus;
using BookingService.Data;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using Polly.Retry;
using Refit;
using SharedService.ServiceBus;
using SharedService.Telemetry;

namespace BookingService.Extensions;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("LocalBookingServiceDb")
            : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
        
        // database
        builder.Services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null
                );
            });
        });
        
        // redis
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            var redisConnectionString = builder.Environment.IsDevelopment()
                ? builder.Configuration.GetConnectionString("LocalRedis")
                : Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
    
            options.Configuration = redisConnectionString;
            options.InstanceName = "BookingService_";
        });
        
        //resilience pipeline
        var maxRetryAttempts = builder.Configuration.GetValue<int>("FlightService:Resilience:RetryCount", 3);
        var retryDelay = builder.Configuration.GetValue<int>("FlightService:Resilience:BaseRetryDelaySeconds", 2);
        var timeout = builder.Configuration.GetValue<int>("FlightService:Resilience:TimeoutSeconds", 30);
        
        builder.Services.AddResiliencePipeline("flight-service-pipeline", resiliencePipelineBuilder =>
        {
            resiliencePipelineBuilder
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = maxRetryAttempts,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(retryDelay),
                    OnRetry = args =>
                    {
                        Console.WriteLine($"FlightServiceClient retry {args.AttemptNumber + 1} after {args.RetryDelay.TotalSeconds}s delay");
                        return ValueTask.CompletedTask;
                    }
                })
                .AddTimeout(TimeSpan.FromSeconds(timeout));
        });
        
        // refit
        builder.Services.AddRefitClient<IFlightClientService>()
            .ConfigureHttpClient(client =>
            {
                var uri = builder.Configuration.GetValue<string>("FlightService:BaseUrl") ?? "http://localhost:5100/";
                var apiKey = builder.Configuration.GetValue<string>("FlightService:ApiKey");
                
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            });
        
        // messaging service bus
        var serviceBusConnectionString = builder.Environment.IsDevelopment()
            ? builder.Configuration.GetConnectionString("AzureServiceBus")
            : Environment.GetEnvironmentVariable("AZURESERVICEBUS_CONNECTION_STRING");

        builder.Services.AddServiceBus(serviceBusConnectionString);
        
        // health checks
        builder.Services.AddHealthChecks()
            .AddCheck("booking-service", () => HealthCheckResult.Healthy())
            .AddSqlServer(connectionString!, name: "booking-service-db")
            .AddCheck("service-bus", () =>
            {
                try
                {
                    var client = new ServiceBusClient(serviceBusConnectionString);
                    return HealthCheckResult.Healthy($"Connected to {client.FullyQualifiedNamespace}");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Failed to connect to Service Bus", ex);
                }
            });
        
        // telemetry
        builder.Services.AddTelemetry(builder.Configuration);

        return builder;
    }
}