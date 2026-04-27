namespace Shared.Contracts.Dtos;

public class SimulationSessionSummaryDto
{
    public int SimulationSessionId { get; set; }
    public int CaseScenarioId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}