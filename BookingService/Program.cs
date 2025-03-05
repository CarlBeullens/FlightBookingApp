using BookingService.Data;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<BookingDbContext>(options =>
// {
//     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseNpgsql(connectionString);
// });

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