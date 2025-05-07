using Azure.Messaging.ServiceBus;

namespace SharedService.Messaging.Services;

public interface IMessageService
{
    Task PublishMessageAsync<T>(string queueName, T payLoad) where T : class;
    
    ServiceBusProcessor GetProcessor(string queueName);
}