namespace Shared.Contracts.Dtos;

public class MedicationOrderDto
{
    public int MedicationOrderId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string Dose { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
}