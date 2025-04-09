using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Messaging.Models;
using Microsoft.Extensions.Logging;

namespace Messaging.Services;

public class MessageService(ServiceBusClient client, ILogger<MessageService> logger) : IMessageService
{
    private readonly ServiceBusClient _client = client;
    private readonly ILogger<MessageService> _logger = logger;

    public async Task PublishMessageAsync<T>(string queueName, T payLoad) where T : class
    {
        var sender = _client.CreateSender(queueName);
        
        await using (sender)
        {
            try
            {
                var eventMessage = new EventMessage
                {
                    Type = typeof(T).Name,
                    Payload = JsonSerializer.Serialize(payLoad)
                };

                var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(eventMessage));

                await sender.SendMessageAsync(serviceBusMessage);

                _logger.LogInformation("Message sent to queue {QueueName}", queueName);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to queue {QueueName}", queueName);
                throw;
            }
        }
    }
}