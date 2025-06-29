using Azure.Messaging.ServiceBus;

namespace SharedService.ServiceBus.Services;

public interface IMessageService
{
    Task PublishMessageAsync<T>(string queueName, T payLoad) where T : class;
    
    ServiceBusProcessor GetProcessor(string queueName);
}