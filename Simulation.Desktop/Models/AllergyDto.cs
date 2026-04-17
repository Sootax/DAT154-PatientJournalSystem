namespace Simulation.Desktop.Models;

public class AllergyDto
{
    public int AllergyId { get; set; }
    public string Substance { get; set; } = string.Empty;
    public string Reaction { get; set; } = string.Empty;
}