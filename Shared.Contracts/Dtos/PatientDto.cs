namespace Shared.Contracts.Dtos;

public class PatientDto
{
    public int PatientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Sex { get; set; } = string.Empty;
    public double WeightKg { get; set; }
    public string MedicalHistory { get; set; } = string.Empty;
    public string CurrentDiagnoses { get; set; } = string.Empty;
}