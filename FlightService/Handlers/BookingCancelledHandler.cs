using FlightService.Services;
using SharedService.Messaging.Models.Booking;
using SharedService.Messaging.Services;

namespace FlightService.Handlers;

public class BookingCancelledHandler(IMessageService messageService, IServiceProvider serviceProvider) 
    : BaseMessageHandler<BookingCancelledEvent>(messageService, serviceProvider, QueueName)
{
    private const string QueueName = "booking-cancelled";
    
    protected override async Task ProcessMessageHandlerAsync(BookingCancelledEvent message, IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();
            
        var result = await flightService.UpdateSeatingAfterCancellationAsync(message.FlightId, message.NumberOfSeats);

        if (result.IsSuccess)
        {
            Console.WriteLine("Update seating after booking cancellation successful: {0} seats are now available", result.Data.ToString());
        }
    }
}