using Shared.Contracts.Dtos;
using System.Net.Http.Json;

namespace CaseSetup.Web.Services
{
    public class CaseApiClient : ICaseApiClient
    {
        private readonly HttpClient _httpClient;

        public CaseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<CaseScenarioDto>> GetAllCasesAsync()
        {
            var cases = await _httpClient.GetFromJsonAsync<List<CaseScenarioDto>>("api/cases");

            return cases ?? [];
        }

        public async Task ActivateAsync(int caseId)
        {
            var response = await _httpClient.PostAsync($"api/cases/{caseId}/activate", null);

            response.EnsureSuccessStatusCode();
        }
    }
}
