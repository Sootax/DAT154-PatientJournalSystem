using Microsoft.AspNetCore.Components;
using Shared.Contracts.Dtos;
using Shared.Contracts.Requests;
using TeacherAssessment.Services;

namespace TeacherAssessment.Components.Pages;

public partial class SessionDetail : ComponentBase, IAsyncDisposable
{
    private const int PollingIntervalSeconds = 5;

    [Parameter] public int SessionId { get; set; }

    [Inject] private ISessionApiClient SessionApi { get; set; } = default!;
    [Inject] private IObservationApiClient ObservationApi { get; set; } = default!;
    [Inject] private TeacherSessionState SessionState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private SimulationSessionDto? _session;
    private string _observationNote = string.Empty;
    private bool _isSubmittingObservation;
    private bool _isEnding;
    private string? _errorMessage;
    private PeriodicTimer? _pollingTimer;
    private CancellationTokenSource? _pollingCts;

    protected override async Task OnInitializedAsync()
    {
        if (!SessionState.IsLoggedIn)
        {
            return;
        }

        await LoadSessionAsync();
        StartPolling();
    }

    private async Task LoadSessionAsync()
    {
        try
        {
            _session = await SessionApi.GetSessionAsync(SessionId);

            if (_session is null)
            {
                _errorMessage = $"Session {SessionId} was not found.";
            }
        }
        catch (HttpRequestException)
        {
            _errorMessage = "Could not reach the backend.";
        }
    }

    private void StartPolling()
    {
        _pollingCts = new CancellationTokenSource();
        _pollingTimer = new PeriodicTimer(TimeSpan.FromSeconds(PollingIntervalSeconds));
        _ = PollLoopAsync(_pollingCts.Token);
    }

    private async Task PollLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (await _pollingTimer!.WaitForNextTickAsync(cancellationToken))
            {
                if (_session?.Status != "Running")
                {
                    break;
                }

                await LoadSessionAsync();
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected on dispose / navigation.
        }
    }

    private async Task SubmitObservationAsync()
    {
        if (string.IsNullOrWhiteSpace(_observationNote) || SessionState.CurrentUser is null)
        {
            return;
        }

        _isSubmittingObservation = true;
        _errorMessage = null;

        try
        {
            await ObservationApi.AddObservationAsync(new AddObservationRequest
            {
                SimulationSessionId = SessionId,
                TeacherUserId = SessionState.CurrentUser.UserId,
                Note = _observationNote,
            });

            _observationNote = string.Empty;
            await LoadSessionAsync();
        }
        catch (HttpRequestException)
        {
            _errorMessage = "Could not save observation. Is the backend running?";
        }
        finally
        {
            _isSubmittingObservation = false;
        }
    }

    private async Task EndSessionAsync()
    {
        _isEnding = true;
        _errorMessage = null;

        try
        {
            var debrief = await SessionApi.EndSessionAsync(SessionId);

            if (debrief is null)
            {
                _errorMessage = "Could not end session.";
                return;
            }

            Navigation.NavigateTo($"/sessions/{SessionId}/debrief");
        }
        catch (HttpRequestException)
        {
            _errorMessage = "Could not reach the backend.";
        }
        finally
        {
            _isEnding = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_pollingCts is not null)
        {
            await _pollingCts.CancelAsync();
            _pollingCts.Dispose();
        }

        _pollingTimer?.Dispose();
    }
}