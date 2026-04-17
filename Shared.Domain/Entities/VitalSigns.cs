namespace Shared.Domain.Entities;

public class VitalSigns
{
    public int VitalSignsId { get; set; }
    public int SystolicBp { get; set; }
    public int DiastolicBp { get; set; }
    public int HeartRate { get; set; }
    public int RespiratoryRate { get; set; }
    public int OxygenSaturation { get; set; }
    public double TemperatureCelsius { get; set; }
    public DateTime RecordedAt { get; set; }
}