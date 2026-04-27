using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.Contracts.Dtos;
using TeacherAssessment.Services;

namespace TeacherAssessment.Components.Pages;

public partial class Debrief : ComponentBase
{
    [Parameter] public int SessionId { get; set; }

    [Inject] private ISessionApiClient SessionApi { get; set; } = default!;
    [Inject] private TeacherSessionState SessionState { get; set; } = default!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

    private DebriefReportDto? _report;
    private bool _isLoading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        if (!SessionState.IsLoggedIn)
        {
            _isLoading = false;
            return;
        }

        try
        {
            _report = await SessionApi.GetDebriefAsync(SessionId);
        }
        catch (HttpRequestException)
        {
            _errorMessage = "Could not reach the backend.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task PrintAsync()
    {
        await JsRuntime.InvokeVoidAsync("window.print");
    }
}