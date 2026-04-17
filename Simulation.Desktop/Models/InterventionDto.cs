namespace Simulation.Desktop.Models;

public class InterventionDto
{
    public int InterventionId { get; set; }
    public int SimulationSessionId { get; set; }
    public int PerformedByUserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? DrugName { get; set; }
    public string? Dose { get; set; }
    public string? Route { get; set; }
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
}