using Shared.Domain.Entities;
using Shared.Infrastructure.Persistence;

namespace Backend.Api.Development;

public static class DevSeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.CaseScenarios.Any())
        {
            return;
        }

        db.CaseScenarios.Add(new CaseScenario
        {
            Title = "Postoperative hypertension",
            Description = "65 y/o patient with elevated BP post-surgery.",
            IsActive = true,
            CreatedByUserId = 1,
            Patient = new Patient
            {
                Name = "Kari Nordmann",
                Age = 65,
                Sex = "F",
                WeightKg = 72,
                MedicalHistory = "Hypertension, type 2 diabetes",
                CurrentDiagnosis = "Post-op hypertension"
            },
            InitialVitals = new VitalSigns
            {
                SystolicBp = 165,
                DiastolicBp = 95,
                HeartRate = 88,
                RespiratoryRate = 18,
                OxygenSaturation = 96,
                TemperatureCelsius = 37.2,
                RecordedAt = DateTime.UtcNow
            },
            Allergies = { new Allergy { Substance = "Penicillin", Reaction = "Rash" } },
            Medications = { new MedicationOrder { DrugName = "Labetalol", Dose = "10 mg", Route = "IV" } },
            Goals = { new CaseGoal { Description = "Reduce systolic BP below 140", TimeLimitSeconds = 600 } }
        });

        db.CaseScenarios.Add(new CaseScenario
        {
            Title = "Sepsis monitoring",
            Description = "54 y/o patient with suspected sepsis.",
            IsActive = false,
            CreatedByUserId = 1,
            Patient = new Patient
            {
                Name = "Ola Hansen",
                Age = 54,
                Sex = "M",
                WeightKg = 85,
                MedicalHistory = "None significant",
                CurrentDiagnosis = "Suspected sepsis"
            },
            InitialVitals = new VitalSigns
            {
                SystolicBp = 95,
                DiastolicBp = 60,
                HeartRate = 115,
                RespiratoryRate = 24,
                OxygenSaturation = 92,
                TemperatureCelsius = 39.1,
                RecordedAt = DateTime.UtcNow
            },
            Goals = { new CaseGoal { Description = "Stabilize BP above 110 systolic", TimeLimitSeconds = 900 } }
        });

        db.SaveChanges();
    }
}