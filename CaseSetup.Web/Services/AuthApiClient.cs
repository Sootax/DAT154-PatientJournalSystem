using Shared.Contracts.Dtos;
using Shared.Contracts.Requests;

namespace CaseSetup.Web.Services;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;
    
    public AuthApiClient(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<UserDto?> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/auth/login", loginRequest);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}
