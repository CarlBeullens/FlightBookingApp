namespace Messaging.Services;

public interface IMessageService
{
    Task PublishMessageAsync<T>(string queueName, T payLoad) where T : class;
}