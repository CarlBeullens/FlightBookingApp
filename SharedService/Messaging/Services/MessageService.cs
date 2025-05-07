using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace SharedService.Messaging.Services;

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
                var message = new ServiceBusMessage(JsonSerializer.Serialize(payLoad));

                await sender.SendMessageAsync(message);

                _logger.LogInformation("Message sent to queue {QueueName}", queueName);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to queue {QueueName}", queueName);
            }
        }
    }

    public ServiceBusProcessor GetProcessor(string queueName)
    {
        var options = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = true
        };
        
        return _client.CreateProcessor(queueName, options);
    }
}