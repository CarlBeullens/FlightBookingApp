using BookingService.Data;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Booking Service API"
    });
});

builder.Services.AddDbContext<BookingDbContext>(options =>
{
    var connectionString = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("Development")
        : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
    
    Console.WriteLine($"Using connection string: {connectionString}");
    
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null
        );
    });
});

builder.Services.AddRefitClient<IFlightClientService>()
    .ConfigureHttpClient(client =>
    {
        var uri = builder.Configuration["FlightService:BaseUrl"] ?? "http://localhost:6000/";
        client.BaseAddress = new Uri(uri);
    });

builder.Services.AddScoped<IBookingService, BookingService.Services.BookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();