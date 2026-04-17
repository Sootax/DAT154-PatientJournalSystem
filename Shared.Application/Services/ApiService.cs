using Shared.Contracts.Dtos;
using System.Net.Http.Json;

namespace Shared.Application.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7127")
        };
    }

    public async Task<CaseScenarioDto?> GetActiveCaseAsync()
    {
        return await _httpClient.GetFromJsonAsync<CaseScenarioDto>($"api/cases/active");
    }

    public async Task<SimulationSessionDto?> StartSessionAsync(int caseId)
    {
        var response = await _httpClient.PostAsync($"api/simulation/start/{caseId}", null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SimulationSessionDto>();
    }

    public async Task<SimulationSessionDto?> GetSessionAsync(int sessionId)
    {
        return await _httpClient.GetFromJsonAsync<SimulationSessionDto>($"api/simulation/{sessionId}");
    }

    public async Task RegisterInteventionAsync(int sessionId, InterventionDto intervention)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/simulation/{sessionId}/intervention", intervention);

        response.EnsureSuccessStatusCode();
    }
}