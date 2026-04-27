using Shared.Contracts.Requests;

namespace TeacherAssessment.Services;

public class ObservationApiClient : IObservationApiClient
{
    private readonly HttpClient _httpClient;

    public ObservationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task AddObservationAsync(AddObservationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/observations", request);

        response.EnsureSuccessStatusCode();
    }
}