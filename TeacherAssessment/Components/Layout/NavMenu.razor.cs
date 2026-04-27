using Microsoft.AspNetCore.Components;
using TeacherAssessment.Services;

namespace TeacherAssessment.Components.Layout;

public partial class NavMenu : ComponentBase, IDisposable
{
    [Inject] private TeacherSessionState SessionState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    protected override void OnInitialized()
    {
        SessionState.OnChange += HandleStateChanged;
    }

    private void HandleStateChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    private void LogoutAsync()
    {
        SessionState.Logout();
        Navigation.NavigateTo("/");
    }

    public void Dispose()
    {
        SessionState.OnChange -= HandleStateChanged;
    }
}