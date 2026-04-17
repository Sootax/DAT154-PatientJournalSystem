using Shared.Domain.Entities;

namespace Shared.Application.Services;

public class DebriefService
{
    public DebriefReport GenerateReport(SimulationSession session)
    {
        var summary = $"Session {session.SimulationSessionId} had "
            + $"{session.Interventions.Count} interventions and "
            + $"{session.Observations.Count} observations.";

        return new DebriefReport
        {
            SimulationSessionId = session.SimulationSessionId,
            GeneratedAt = DateTime.UtcNow,
            Summary = summary,
        };
    }
}