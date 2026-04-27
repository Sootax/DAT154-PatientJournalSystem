using Shared.Contracts.Dtos;

namespace TeacherAssessment.Services;

/// <summary>
/// Per-circuit state for the currently logged-in user.
/// Registered as scoped so each Blazor Server circuit (browser tab) gets its own instance.
/// </summary>
public class TeacherSessionState
{
    public UserDto? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser is not null;

    public bool IsTeacher => CurrentUser?.Role == "Teacher";

    public event Action? OnChange;

    public void SetUser(UserDto user)
    {
        CurrentUser = user;
        OnChange?.Invoke();
    }

    public void Logout()
    {
        CurrentUser = null;
        OnChange?.Invoke();
    }
}