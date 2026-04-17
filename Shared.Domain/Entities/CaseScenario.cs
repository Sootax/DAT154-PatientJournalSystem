namespace Shared.Domain.Entities;

public class CaseScenario
{
    public int CaseScenarioId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int CreatedByUserId { get; set; }

    public Patient? Patient { get; set; }
    public VitalSigns? InitialVitals { get; set; }
    public List<MedicationOrder> Medications { get; set; } = [];
    public List<Allergy> Allergies { get; set; } = [];
    public List<CaseGoal> Goals { get; set; } = [];
}