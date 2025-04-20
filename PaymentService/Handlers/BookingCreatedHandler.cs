using PaymentService.Services;
using Shared.Messaging.Models.Booking;
using Shared.Messaging.Services;

namespace PaymentService.Handlers;

public class BookingCreatedHandler(IMessageService messageService, IServiceProvider serviceProvider) 
    : BaseMessageHandler<BookingCreatedEvent>(messageService, serviceProvider, QueueName)
{
    private const string QueueName = "booking-created";
    
    protected override async Task ProcessMessageHandlerAsync(BookingCreatedEvent message, IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

        _ = await paymentService.CreatePaymentAsync(message.BookingId);
    }
}