using Shared.Domain.Entities;

namespace Shared.Application.Services;

public class SimulationEngine
{
    public VitalSignsRecord ApplyInterventionEffect(VitalSignsRecord currentVitals, Intervention intervention)
    {
        var updated = new VitalSignsRecord
        {
            SimulationSessionId = currentVitals.SimulationSessionId,
            Timestamp = DateTime.UtcNow,
            SystolicBp = currentVitals.SystolicBp,
            DiastolicBp = currentVitals.DiastolicBp,
            HeartRate = currentVitals.HeartRate,
            RespiratoryRate = currentVitals.RespiratoryRate,
            OxygenSaturation = currentVitals.OxygenSaturation,
            TemperatureCelsius = currentVitals.TemperatureCelsius,
        };

        if (intervention.Type == "Medication" && intervention.DrugName?.ToLower() == "labetalol")
        {
            updated.SystolicBp -= 10;
            updated.DiastolicBp -= 5;
            updated.HeartRate -= 5;
        }

        if (intervention.Type == "Oxygen")
        {
            updated.OxygenSaturation += 2;
        }

        return updated;
    }
}
