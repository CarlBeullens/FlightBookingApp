using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ApiGateway"));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]
                                ?? throw new InvalidOperationException("JWT secret key not found")))
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"))
    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", async (HttpContext context) => {
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");
    var content = await File.ReadAllTextAsync(filePath);
    await context.Response.WriteAsync(content);
});

app.MapGet("/test-auth", 
        (ClaimsPrincipal user) => $"Hello, {user.FindFirstValue(ClaimTypes.Name)}! You are authenticated.")
    .RequireAuthorization()
    .Produces<string>(StatusCodes.Status200OK)
    .Produces<string>(StatusCodes.Status401Unauthorized);

app.MapGet("/test-customer-only", () => "Customer area")
    .RequireAuthorization("RequireCustomerRole")
    .Produces<string>(StatusCodes.Status200OK)
    .Produces<string>(StatusCodes.Status403Forbidden);

app.MapGet("/test-admin-only", () => "Admin area")
    .RequireAuthorization("RequireAdminRole")
    .Produces<string>(StatusCodes.Status200OK)
    .Produces<string>(StatusCodes.Status403Forbidden);

app.MapReverseProxy();

await app.RunAsync();