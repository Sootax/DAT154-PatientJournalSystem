namespace Shared.Domain.Entities;

public class DebriefReport
{
    public int DebriefReportId { get; set; }
    public int SimulationSessionId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string Summary { get; set; } = string.Empty;
}