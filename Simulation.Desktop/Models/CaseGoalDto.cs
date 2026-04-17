namespace Simulation.Desktop.Models;

public class CaseGoalDto
{
    public int CaseGoalId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int TimeLimitSeconds { get; set; }
}