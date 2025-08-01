using FlightService.Services;
using SharedService.ServiceBus.EventMessages.Booking;
using SharedService.ServiceBus.Services;

namespace FlightService.Handlers;

public class BookingConfirmedHandler(IMessageService messageService, IServiceProvider serviceProvider) 
    : BaseMessageHandler<BookingConfirmedEvent>(messageService, serviceProvider, QueueName)
{
    private const string QueueName = "booking-confirmed";
    
    protected override async Task ProcessMessageHandlerAsync(BookingConfirmedEvent message, IServiceProvider serviceProvider, CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        
        var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();
            
        var result = await flightService.UpdateSeatingAfterConfirmationAsync(message.FlightId, message.NumberOfSeats);

        if (result.IsSuccess)
        {
            Console.WriteLine("Update seating after booking confirmation successful: {0} seats are now available", result.Data.ToString());
        }
    }
}