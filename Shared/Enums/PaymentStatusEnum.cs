using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Enums;

/// <summary>
/// Represents the payment status of a booking
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentStatusEnum
{
    /// <summary>
    /// Initial state, no payment attempt made
    /// </summary>
    [EnumMember(Value = "None")]
    None = 0,
        
    /// <summary>
    /// Payment has been initiated but not completed
    /// </summary>
    [EnumMember(Value = "Pending")]
    Pending = 1,
        
    /// <summary>
    /// Payment has been successfully processed
    /// </summary>
    [EnumMember(Value = "Paid")]
    Paid = 2,
        
    /// <summary>
    /// Payment has been cancelled
    /// </summary>
    [EnumMember(Value = "Cancelled")]
    Cancelled = 3,
        
    /// <summary>
    /// Payment processing failed
    /// </summary>
    [EnumMember(Value = "Failed")]
    Failed = 4,
        
    /// <summary>
    /// Payment has been refunded
    /// </summary>
    [EnumMember(Value = "Refunded")]
    Refunded = 5
}