namespace Shared.Domain.Entities;

public class CaseGoal
{
    public int CaseGoalId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int TimeLimitSeconds { get; set; }
}