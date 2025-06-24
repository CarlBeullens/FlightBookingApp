using Azure.Messaging.ServiceBus;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaymentService.Data;
using PaymentService.Handlers;
using PaymentService.Services;
using SharedService.Security;
using SharedService.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(title: "Payment Service API");

var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("LocalPaymentServiceDb")
    : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

builder.Services.AddDbContext<PaymentDbContext>(options =>
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

var serviceBusConnectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("AzureServiceBus")
    : Environment.GetEnvironmentVariable("AZURESERVICEBUS_CONNECTION_STRING");

builder.Services.AddServiceBus(serviceBusConnectionString);

builder.Services.AddHostedService<BookingCreatedHandler>();
builder.Services.AddHostedService<RefundPaymentHandler>();

builder.Services.AddScoped<IPaymentService, PaymentService.Services.PaymentService>();

builder.Services.AddHealthChecks()
    .AddCheck("payment-service", () => HealthCheckResult.Healthy())
    .AddSqlServer(connectionString!, name: "payment-service-db")
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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
        var context = services.GetRequiredService<PaymentDbContext>();
        await context.Database.MigrateAsync();
        app.Logger.LogInformation("Database migrated successfully or already up to date");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
    }
}

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.UseApiKeyAuthentication();

await app.RunAsync();