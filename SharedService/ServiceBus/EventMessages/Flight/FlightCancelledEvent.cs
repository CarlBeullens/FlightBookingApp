namespace SharedService.ServiceBus.EventMessages.Flight;

public class FlightCancelledEvent
{
    public Guid FlightId { get; set; }
}