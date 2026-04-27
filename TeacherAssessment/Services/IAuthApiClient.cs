using Shared.Contracts.Dtos;
using Shared.Contracts.Requests;

namespace TeacherAssessment.Services;

public interface IAuthApiClient
{
    Task<UserDto?> LoginAsync(LoginRequest loginRequest);
}