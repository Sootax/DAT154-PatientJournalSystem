namespace Shared.Domain.Entities;

public class TimelineEvent
{
    public int TimelineEventId { get; set; }
    public int SimulationSessionId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; } = string.Empty;
}