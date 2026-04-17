namespace Simulation.Desktop.Models;

public class SimulationSessionDto
{
    public int SimulationSessionId { get; set; }
    public int CaseScenarioId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<InterventionDto> Interventions { get; set; } = [];
    public List<VitalSignsRecordDto> VitalHistory { get; set; } = [];
    public List<TimelineEventDto> TimelineEvents { get; set; } = [];
}