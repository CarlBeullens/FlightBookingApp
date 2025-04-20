using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PaymentService.Data;
using PaymentService.Handlers;
using PaymentService.Services;
using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payment Service API",
        Version = "v1"
    });
});

builder.Services.AddDbContext<PaymentDbContext>(options =>
{
    var connectionString = builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("LocalPaymentServiceDb")
        : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            
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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
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
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();