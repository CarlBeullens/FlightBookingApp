using PaymentService.Models;
using PaymentService.Services;
using SharedService.Messaging.Models.Booking;
using SharedService.Messaging.Services;

namespace PaymentService.Handlers;

public class RefundPaymentHandler(IMessageService messageService, IServiceProvider serviceProvider) 
    : BaseMessageHandler<RefundPaymentCommand>(messageService, serviceProvider, QueueName)
{
    private const string QueueName = "booking-cancelled-refund-payment";
    
    protected override async Task ProcessMessageHandlerAsync(RefundPaymentCommand message, IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();

        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        
        _ = await paymentService.SetPaymentStatus(message.BookingId, PaymentStatus.Refunded);
    }
}