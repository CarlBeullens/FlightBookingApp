using FlightService.Configuration;
using FlightService.Data;
using FlightService.EventHandlers;
using FlightService.Services;
using Microsoft.EntityFrameworkCore;
using Refit;
using SharedService.Messaging;
using SharedService.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger("Flight Service API", "v1");

var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("LocalFlightServiceDb")
    : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

builder.Services.AddDbContext<FlightServiceDbContext>(options =>
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

// Register the Amadeus auth API
builder.Services.AddRefitClient<IAmadeusAuthApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["AmadeusBaseUrl"] ?? "https://test.api.amadeus.com");
        c.Timeout = TimeSpan.FromSeconds(10);
    });

// Register the Amadeus services API
builder.Services.AddRefitClient<IAmadeusApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["AmadeusBaseUrl"] ?? "https://test.api.amadeus.com");
        c.Timeout = TimeSpan.FromSeconds(10);
    });

var serviceBusConnectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("AzureServiceBus")
    : Environment.GetEnvironmentVariable("AZURESERVICEBUS_CONNECTION_STRING");

builder.Services.AddServiceBus(serviceBusConnectionString);

builder.Services.AddScoped<IFlightService, FlightService.Services.FlightService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();

builder.Services.AddHostedService<BookingConfirmedHandler>();
builder.Services.AddHostedService<BookingCancelledHandler>();

builder.Services.Configure<AmadeusSettings>(
    builder.Configuration.GetSection(AmadeusSettings.Token));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FlightServiceDbContext>();
        app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
        await context.Database.MigrateAsync();
        app.Logger.LogInformation("Database migrated successfully or already up to date");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

await app.RunAsync();