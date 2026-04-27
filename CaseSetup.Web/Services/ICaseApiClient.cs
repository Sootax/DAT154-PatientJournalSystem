using Shared.Contracts.Dtos;

namespace CaseSetup.Web.Services;

public interface ICaseApiClient
{
    Task<IReadOnlyList<CaseScenarioDto>> GetAllCasesAsync();

    Task ActivateAsync(int caseId);

    Task<CaseScenarioDto?> GetCaseByIdAsync(int id);
}
