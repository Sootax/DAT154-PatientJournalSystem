namespace Simulation.Desktop.Models;

public class TimelineEventDto
{
    public int TimelineEventId { get; set; }
    public int SimulationSessionId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; } = string.Empty;
}