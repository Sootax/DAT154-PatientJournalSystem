using Shared.Contracts.Dtos;

namespace TeacherAssessment.Services;

public interface ISessionApiClient
{
    Task<IReadOnlyList<SimulationSessionSummaryDto>> GetSessionsAsync();

    Task<SimulationSessionDto?> GetSessionAsync(int sessionId);

    Task<DebriefReportDto?> EndSessionAsync(int sessionId);

    Task<DebriefReportDto?> GetDebriefAsync(int sessionId);
}