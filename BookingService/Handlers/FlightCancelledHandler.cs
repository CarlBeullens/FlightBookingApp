using BookingService.Services;
using Shared.Messaging.Models.Flight;
using Shared.Messaging.Services;

namespace BookingService.EventHandlers;

public class FlightCancelledHandler(IMessageService messageService, IServiceProvider serviceProvider) 
    : BaseMessageHandler<FlightCancelledEvent>(messageService, serviceProvider, QueueName)
{
    private const string QueueName = "flight-cancelled";
    
    protected override async Task ProcessMessageHandlerAsync(FlightCancelledEvent message, IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

        var result = await bookingService.UpdateBookingsAfterCancelledFlightAsync(message.FlightId);
        
        Console.WriteLine("{0} bookings updated after flight cancellation", result.Data.ToString());
    }
}