using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging;

public static class StartupExtensions
{
    public static IServiceCollection AddServiceBus(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Service Bus connection string is required.");
        }
        
        services.AddAzureClients(client =>
        {
            client.AddServiceBusClient(connectionString);
        });

        return services;
    }
}