namespace Shared.Contracts.Dtos;

public class CaseScenarioDto
{
    public int CaseScenarioId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int CreatedByUserId { get; set; }
    public PatientDto? Patient { get; set; }
    public VitalSignsDto? InitialVitals { get; set; }
    public List<AllergyDto>? Allergies { get; set; }
    public List<MedicationOrderDto> Medications { get; set; } = [];
    public List<CaseGoalDto> Goals { get; set; } = [];
}