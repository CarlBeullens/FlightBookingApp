namespace Messaging.Models;

public class EventMessage
{
    public required string Type { get; set; }
    
    public required string Payload { get; set; }
}