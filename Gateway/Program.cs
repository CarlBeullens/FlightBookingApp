var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ApiGateway"));

var app = builder.Build();

app.MapGet("/", async (HttpContext context) => {
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");
    var content = await File.ReadAllTextAsync(filePath);
    await context.Response.WriteAsync(content);
});

app.MapReverseProxy();

app.Run();