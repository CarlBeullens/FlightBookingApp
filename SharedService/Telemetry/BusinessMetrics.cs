using System.Diagnostics.Metrics;

namespace SharedService.Telemetry;

internal sealed class BusinessMetrics : IBusinessMetrics
{
    private readonly Counter<int> _bookingsCreated;
    private readonly Counter<int> _bookingsConfirmed;
    private readonly Counter<int> _bookingsCancelled;
    
    public BusinessMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("FlightBookingService", "1.0.0");
        
        _bookingsCreated = meter.CreateCounter<int>("bookings.created");
        _bookingsConfirmed = meter.CreateCounter<int>("bookings.confirmed");
        _bookingsCancelled = meter.CreateCounter<int>("bookings.cancelled");
    }
    
    public void RecordBookingCreated(string bookingId, string flightNumber, decimal amount)
    {
        _bookingsCreated.Add(1, 
            new KeyValuePair<string, object?>("booking_id", bookingId),
            new KeyValuePair<string, object?>("flight_number", flightNumber),
            new KeyValuePair<string, object?>("amount", amount));
    }
    
    public void RecordBookingConfirmed(string bookingId)
    {
        _bookingsConfirmed.Add(1, new KeyValuePair<string, object?>("booking_id", bookingId));
    }
    
    public void RecordBookingCancelled(string bookingId)
    {
        _bookingsCancelled.Add(1, new KeyValuePair<string, object?>("booking_id", bookingId));
    }
}