namespace Shared.Domain.Entities;

public class SimulationSession
{
    public int SimulationSessionId { get; set; }
    public int CaseScenarioId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Status { get; set; } = "Running";

    public CaseScenario? CaseScenario { get; set; }
    public List<Intervention> Interventions { get; set; } = [];
    public List<VitalSignsRecord> VitalHistory { get; set; } = [];
    public List<TimelineEvent> TimelineEvents { get; set; } = [];
    public List<TeacherObservation> Observations { get; set; } = [];
}