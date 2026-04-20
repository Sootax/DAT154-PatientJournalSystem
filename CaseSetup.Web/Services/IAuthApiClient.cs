using Shared.Contracts.Requests;
using Shared.Contracts.Dtos;

namespace CaseSetup.Web.Services;

public interface IAuthApiClient
{
    Task<UserDto?> LoginAsync(LoginRequest loginRequest);
}