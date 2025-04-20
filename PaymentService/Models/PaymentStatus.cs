namespace PaymentService.Models;

public class PaymentStatus
{
    public const string None = "None";
    public const string Pending = "Pending";
    public const string Paid = "Paid";
    public const string Cancelled = "Cancelled";
    public const string Failed = "Failed";
    public const string Refunded = "Refunded";
}