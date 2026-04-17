namespace Shared.Domain.Entities;

public class Allergy
{
    public int AllergyId { get; set; }
    public string Substance { get; set; } = string.Empty;
    public string Reaction { get; set; } = string.Empty;
}