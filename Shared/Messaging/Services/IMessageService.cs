using Azure.Messaging.ServiceBus;

namespace Shared.Messaging.Services;

public interface IMessageService
{
    Task PublishMessageAsync<T>(string queueName, T payLoad) where T : class;
    
    ServiceBusProcessor GetProcessor(string queueName);
}