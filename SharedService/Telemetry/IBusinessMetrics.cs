namespace SharedService.Telemetry;

public interface IBusinessMetrics
{
    void RecordBookingCreated(string bookingId, string flightNumber, decimal amount);

    void RecordBookingConfirmed(string bookingId);

    void RecordBookingCancelled(string bookingId);
}