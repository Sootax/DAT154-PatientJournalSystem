using System.Net;
using Shared.Contracts.Dtos;

namespace TeacherAssessment.Services;

public class SessionApiClient : ISessionApiClient
{
    private readonly HttpClient _httpClient;

    public SessionApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<SimulationSessionSummaryDto>> GetSessionsAsync()
    {
        var sessions = await _httpClient.GetFromJsonAsync<List<SimulationSessionSummaryDto>>("api/simulation");

        return sessions ?? [];
    }

    public async Task<SimulationSessionDto?> GetSessionAsync(int sessionId)
    {
        var response = await _httpClient.GetAsync($"api/simulation/{sessionId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SimulationSessionDto>();
    }

    public async Task<DebriefReportDto?> EndSessionAsync(int sessionId)
    {
        var response = await _httpClient.PostAsync($"api/simulation/{sessionId}/end", null);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DebriefReportDto>();
    }

    public async Task<DebriefReportDto?> GetDebriefAsync(int sessionId)
    {
        var response = await _httpClient.GetAsync($"api/simulation/{sessionId}/debrief");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<DebriefReportDto>();
    }
}