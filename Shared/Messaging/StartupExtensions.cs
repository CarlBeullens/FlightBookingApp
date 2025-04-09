using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.Services;

namespace Shared.Messaging;

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
        
        services.AddSingleton<IMessageService, MessageService>();

        return services;
    }
}