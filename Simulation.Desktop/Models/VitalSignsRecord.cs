namespace Simulation.Desktop.Models;

public class VitalSignsRecordDto
{
    public int VitalSignsRecordId { get; set; }
    public int SimulationSessionId { get; set; }
    public DateTime Timestamp { get; set; }
    public int SystolicBp { get; set; }
    public int DiastolicBp { get; set; }
    public int HeartRate { get; set; }
    public int RespiratoryRate { get; set; }
    public int OxygenSaturation { get; set; }
    public double TemperatureCelsius { get; set; }
}