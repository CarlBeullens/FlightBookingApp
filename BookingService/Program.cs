using BookingService.Data;
using BookingService.EventHandlers;
using BookingService.Extensions;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Booking Service API",
        Version = "v1"
    });
});

builder.AddInfrastructure();

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
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();