using AuthService.Data;
using AuthService.Extensions;
using AuthService.Models;
using AuthService.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedService.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(title: "Security Service API");

var connectionString = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("LocalSecurityServiceDb")
    : Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

builder.Services.AddDbContext<IdentityDbContext>(options =>
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

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    options.User.RequireUniqueEmail = true;
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

builder.Services.AddHealthChecks()
    .AddCheck("auth-service", () => HealthCheckResult.Healthy())
    .AddSqlServer(connectionString!, name: "identity-service-db");

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.AddAuthEndPoints();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        app.Logger.LogInformation("Using connection string: {ConnectionString}", connectionString);
        var context = services.GetRequiredService<IdentityDbContext>();
        await context.Database.MigrateAsync();
        app.Logger.LogInformation("Database migrated successfully or already up to date");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
    }
}

await app.RunAsync();