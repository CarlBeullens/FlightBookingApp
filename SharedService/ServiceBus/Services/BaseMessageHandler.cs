using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;

namespace SharedService.ServiceBus.Services;

public abstract class BaseMessageHandler<TMessage>(IMessageService messageService, IServiceProvider serviceProvider, string queueName) : BackgroundService
{
    private readonly IMessageService _messageService = messageService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly string _queueName = queueName;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _messageService.GetProcessor(_queueName);

        processor.ProcessMessageAsync += HandleMessageAsync;
        processor.ProcessErrorAsync += HandleErrorAsync;
        
        await processor.StartProcessingAsync(stoppingToken);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
    
    private async Task HandleMessageAsync(ProcessMessageEventArgs args)
    {
        var receivedMessage = args.Message;
    
        var payLoad = JsonSerializer.Deserialize<TMessage>(receivedMessage.Body.ToString());

        if (payLoad is not null)
        {
            await ProcessMessageHandlerAsync(payLoad, _serviceProvider, args.CancellationToken);
        }
    }

    private static Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error processing message: {args.Exception.Message}");
        
        return Task.CompletedTask;
    }
    
    protected abstract Task ProcessMessageHandlerAsync(TMessage message, IServiceProvider serviceProvider, CancellationToken stoppingToken);
}