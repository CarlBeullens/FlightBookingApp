using BookingService.Data;
using BookingService.Extensions;
using BookingService.Handlers;
using BookingService.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using SharedService.Security;
using SharedService.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddStructuredLogging();
builder.AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(title: "Booking Service API");

builder.Services.AddScoped<IBookingService, BookingService.Services.BookingService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddHostedService<FlightCancelledHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BookingDbContext>();
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