using Shared.Application.Services;
using Shared.Contracts.Dtos;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Simulation.Desktop;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService = new();
    private readonly DispatcherTimer _autoRefreshTimer = new();
    private CaseScenarioDto? _activeCase;
    private SimulationSessionDto? _currentSession;

    public MainWindow()
    {
        InitializeComponent();
        InitializeUiState();
        InitializeAutoRefresh();
    }

    private void InitializeUiState()
    {
        StartSessionButton.IsEnabled = false;
        RefreshSessionButton.IsEnabled = false;
        RegisterInterventionButton.IsEnabled = false;
        TypeComboBox.SelectedIndex = 0;

        SessionIdText.Text = "Session ID: -";
        SessionStatusText.Text = "Status: -";
        SessionStartedText.Text = "Started: -";
        SessionEndedText.Text = "Ended: -";
        CurrentCaseStatusText.Text = "Case loaded: No";
        AutoRefreshStatusText.Text = "Auto-refresh: Off";

        PatientNameText.Text = "Name: -";
        PatientAgeText.Text = "Age: -";
        PatientSexText.Text = "Sex: -";
        PatientWeightText.Text = "Weight: -";
        DiagnosisText.Text = "Diagnosis: -";
        MedicalHistoryText.Text = "Medical history: -";
        CaseTitleText.Text = "Case: -";

        BpText.Text = "BP: -";
        HrText.Text = "HR: -";
        RrText.Text = "RR: -";
        Spo2Text.Text = "SpO2: -";
        TempText.Text = "Temp: -";

        TimelineListBox.Items.Clear();
        InterventionsListBox.Items.Clear();
        AllergiesListBox.Items.Clear();
        MedicationsListBox.Items.Clear();
        GoalsListBox.Items.Clear();
    }

    private void InitializeAutoRefresh()
    {
        _autoRefreshTimer.Interval = TimeSpan.FromSeconds(5);
        _autoRefreshTimer.Tick += AutoRefreshTimer_Tick;
    }

    private async void AutoRefreshTimer_Tick(object? sender, EventArgs e)
    {
        if (_currentSession is not null)
        {
            await RefreshSessionAsync(showMessageOnMissingSession: false);
        }
    }

    private async void LoadCaseButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _activeCase = await _apiService.GetActiveCaseAsync();

            if (_activeCase is null)
            {
                MessageBox.Show("No active case found.");
                return;
            }

            ShowPatientDetails(_activeCase);
            ShowInitialVitals(_activeCase);
            ShowCaseCollections(_activeCase);

            TimelineListBox.Items.Clear();
            InterventionsListBox.Items.Clear();
            TimelineListBox.Items.Add("Active case loaded.");

            StartSessionButton.IsEnabled = true;
            RefreshSessionButton.IsEnabled = true;
            RegisterInterventionButton.IsEnabled = false;

            SessionIdText.Text = "Session ID: -";
            SessionStatusText.Text = "Status: Not started";
            SessionStartedText.Text = "Started: -";
            SessionEndedText.Text = "Ended: -";
            CurrentCaseStatusText.Text = "Case loaded: Yes";

            _currentSession = null;
            _autoRefreshTimer.Stop();
            AutoRefreshStatusText.Text = "Auto-refresh: Off";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to load active case: {ex.Message}");
        }
    }

    private async void StartSessionButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_activeCase is null)
            {
                MessageBox.Show("Load the active case first.");
                return;
            }

            _currentSession = await _apiService.StartSessionAsync(_activeCase.CaseScenarioId);

            if (_currentSession is null)
            {
                MessageBox.Show("Failed to start session.");
                return;
            }

            UpdateSessionInfo();

            RefreshSessionButton.IsEnabled = true;
            RegisterInterventionButton.IsEnabled = true;
            StartSessionButton.IsEnabled = false;

            _autoRefreshTimer.Start();
            AutoRefreshStatusText.Text = "Auto-refresh: On (5s)";

            await RefreshSessionAsync(showMessageOnMissingSession: false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start session: {ex.Message}");
        }
    }

    private async void RefreshSessionButton_Click(object sender, RoutedEventArgs e)
    {
        await RefreshSessionAsync(showMessageOnMissingSession: true);
    }

    private async Task RefreshSessionAsync(bool showMessageOnMissingSession)
    {
        try
        {
            if (_currentSession is null)
            {
                if (showMessageOnMissingSession)
                {
                    MessageBox.Show("No active session.");
                }

                return;
            }

            var session = await _apiService.GetSessionAsync(_currentSession.SimulationSessionId);

            if (session is null)
            {
                if (showMessageOnMissingSession)
                {
                    MessageBox.Show("Could not load session.");
                }

                return;
            }

            _currentSession = session;
            UpdateSessionInfo();

            var latestVitals = _currentSession.VitalHistory
                .OrderByDescending(vital => vital.Timestamp)
                .FirstOrDefault();

            if (latestVitals is not null)
            {
                ShowVitals(latestVitals);
            }

            TimelineListBox.Items.Clear();

            foreach (var timelineEvent in _currentSession.TimelineEvents.OrderBy(t => t.Timestamp))
            {
                TimelineListBox.Items.Add(
                    $"{timelineEvent.Timestamp:HH:mm:ss} - " +
                    $"{timelineEvent.Description}");
            }

            InterventionsListBox.Items.Clear();

            foreach (var intervention in _currentSession.Interventions.OrderBy(i => i.Timestamp))
            {
                var detail = !string.IsNullOrWhiteSpace(intervention.DrugName)
                    ? intervention.DrugName
                    : intervention.Description ?? "-";

                var dose = string.IsNullOrWhiteSpace(intervention.Dose)
                    ? string.Empty
                    : $" ({intervention.Dose})";

                InterventionsListBox.Items.Add($"{intervention.Timestamp:HH:mm:ss} - " + $"{intervention.Type} - {detail}{dose}");
            }

            if (_currentSession.Status == "Ended")
            {
                RegisterInterventionButton.IsEnabled = false;
                RefreshSessionButton.IsEnabled = true;
                _autoRefreshTimer.Stop();
                AutoRefreshStatusText.Text = "Auto-refresh: Off";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to refresh session: {ex.Message}");
        }
    }

    private async void RegisterInterventionButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_currentSession is null)
            {
                MessageBox.Show("Start a session first.");
                return;
            }

            var selectedType = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var drugName = string.IsNullOrWhiteSpace(DrugNameTextBox.Text)
                ? null
                : DrugNameTextBox.Text.Trim();

            var dose = string.IsNullOrWhiteSpace(DoseTextBox.Text)
                ? null
                : DoseTextBox.Text.Trim();

            var route = string.IsNullOrWhiteSpace(RouteTextBox.Text)
                ? null
                : RouteTextBox.Text.Trim();

            var description = string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                ? null
                : DescriptionTextBox.Text.Trim();


            if (selectedType is null)
            {
                MessageBox.Show("No selected type found.");
                return;
            }

            var validationError = ValidateInterventionInput(selectedType, drugName, dose, route, description);

            if (validationError is not null)
            {
                MessageBox.Show(validationError);
                return;
            }

            var intervention = new InterventionDto
            {
                PerformedByUserId = 2,
                Type = selectedType,
                DrugName = drugName,
                Dose = dose,
                Route = route,
                Description = description,
            };

            await _apiService.RegisterInteventionAsync(_currentSession.SimulationSessionId, intervention);

            ClearInterventionInputs();

            await RefreshSessionAsync(showMessageOnMissingSession: false);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private static string? ValidateInterventionInput(string type, string? drugName, string? dose, string? route, string? description)
    {
        switch (type)
        {
            case "Medication":
                if (string.IsNullOrWhiteSpace(drugName))
                {
                    return "Medication requires a drug name.";
                }

                if (string.IsNullOrWhiteSpace(dose))
                {
                    return "Medication requires a dose.";
                }

                if (string.IsNullOrWhiteSpace(route))
                {
                    return "Medication requires a route.";
                }

                break;

            case "Fluid":
                if (string.IsNullOrWhiteSpace(drugName) && string.IsNullOrWhiteSpace(description))
                {
                    return "Fluid administration requires a fluid name or description.";
                }

                if (string.IsNullOrWhiteSpace(dose))
                {
                    return "Fluid administration requires an amount.";
                }

                if (string.IsNullOrWhiteSpace(route))
                {
                    return "Fluid administration requires a route.";
                }

                break;

            case "Oxygen":
                if (string.IsNullOrWhiteSpace(description) && string.IsNullOrWhiteSpace(dose))
                {
                    return "Oxygen intervention requires a description or flow value.";
                }

                break;

            case "Other":
                if (string.IsNullOrWhiteSpace(description))
                {
                    return "This intervention type requires a description.";
                }

                break;

            default:
                return "Unknown intervention type selected.";
        }

        return null;
    }

    private void ShowPatientDetails(CaseScenarioDto caseScenario)
    {
        PatientNameText.Text = $"Name: {caseScenario.Patient?.Name ?? "-"}";
        PatientAgeText.Text = $"Age: {(caseScenario.Patient is null ? "-" : caseScenario.Patient.Age)}";
        PatientSexText.Text = $"Sex: {caseScenario.Patient?.Sex ?? "-"}";
        PatientWeightText.Text = $"Weight: {(caseScenario.Patient is null ? "-" : caseScenario.Patient.WeightKg)}";
        DiagnosisText.Text = $"Diagnosis: {caseScenario.Patient?.CurrentDiagnoses ?? "-"}";
        MedicalHistoryText.Text = $"Medical history: {caseScenario.Patient?.MedicalHistory ?? "-"}";
        CaseTitleText.Text = $"Case: {caseScenario.Title}";
    }

    private void ShowCaseCollections(CaseScenarioDto caseScenario)
    {
        AllergiesListBox.Items.Clear();
        MedicationsListBox.Items.Clear();
        GoalsListBox.Items.Clear();

        if (caseScenario.Allergies.Count == 0)
        {
            AllergiesListBox.Items.Add("No allergies registered.");
        }
        else
        {
            foreach (var allergy in caseScenario.Allergies)
            {
                AllergiesListBox.Items.Add($"{allergy.Substance} - {allergy.Reaction}");
            }
        }

        if (caseScenario.Medications.Count == 0)
        {
            MedicationsListBox.Items.Add("No medications registered.");
        }
        else
        {
            foreach (var medication in caseScenario.Medications)
            {
                MedicationsListBox.Items.Add($"{medication.DrugName}, {medication.Dose}, {medication.Route}");
            }
        }

        if (caseScenario.Goals.Count == 0)
        {
            GoalsListBox.Items.Add("No goals registered.");
        }
        else
        {
            foreach (var goal in caseScenario.Goals)
            {
                GoalsListBox.Items.Add($"{goal.Description} ({goal.TimeLimitSeconds}s)");
            }
        }
    }

    private void ShowInitialVitals(CaseScenarioDto caseScenario)
    {
        if (caseScenario.InitialVitals is null)
        {
            BpText.Text = "BP: -";
            HrText.Text = "HR: -";
            RrText.Text = "RR: -";
            Spo2Text.Text = "SpO2: -";
            TempText.Text = "Temp: -";

            return;
        }

        ShowVitals(new VitalSignsRecordDto
        {
            SystolicBp = caseScenario.InitialVitals.SystolicBp,
            DiastolicBp = caseScenario.InitialVitals.DiastolicBp,
            HeartRate = caseScenario.InitialVitals.HeartRate,
            RespiratoryRate = caseScenario.InitialVitals.RespiratoryRate,
            OxygenSaturation = caseScenario.InitialVitals.OxygenSaturation,
            TemperatureCelsius = caseScenario.InitialVitals.TemperatureCelsius,
        });
    }

    private void UpdateSessionInfo()
    {
        if (_currentSession is null)
        {
            SessionIdText.Text = "Session ID: -";
            SessionStatusText.Text = "Status: -";
            SessionStartedText.Text = "Started: -";
            SessionEndedText.Text = "Ended: -";

            return;
        }

        SessionIdText.Text = $"Session ID: {_currentSession.SimulationSessionId}";

        SessionStatusText.Text = $"Status: {_currentSession.Status}";

        SessionStartedText.Text = $"Started: {_currentSession.StartedAt:yyyy-MM-dd HH:mm:ss}";

        SessionEndedText.Text = _currentSession.EndedAt.HasValue
            ? $"Ended: {_currentSession.EndedAt:yyyy-MM-dd HH:mm:ss}"
            : "Ended: -";
    }

    private void ShowVitals(VitalSignsRecordDto vitals)
    {
        BpText.Text = $"BP: {vitals.SystolicBp}/{vitals.DiastolicBp}";
        HrText.Text = $"HR: {vitals.HeartRate}";
        RrText.Text = $"RR: {vitals.RespiratoryRate}";
        Spo2Text.Text = $"SpO2: {vitals.OxygenSaturation}%";
        TempText.Text = $"Temp: {vitals.TemperatureCelsius:F1} °C";
    }

    private void ClearInterventionInputs()
    {
        TypeComboBox.SelectedIndex = 0;
        DrugNameTextBox.Text = string.Empty;
        DoseTextBox.Text = string.Empty;
        RouteTextBox.Text = string.Empty;
        DescriptionTextBox.Text = string.Empty;
    }

    protected override void OnClosed(EventArgs e)
    {
        _autoRefreshTimer.Stop();
        base.OnClosed(e);
    }
}