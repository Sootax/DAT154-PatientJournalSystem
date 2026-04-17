using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;

namespace Shared.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<CaseScenario> CaseScenarios => Set<CaseScenario>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<VitalSigns> VitalSigns => Set<VitalSigns>();
    public DbSet<MedicationOrder> MedicationOrders => Set<MedicationOrder>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<CaseGoal> CaseGoals => Set<CaseGoal>();
    public DbSet<SimulationSession> SimulationSessions => Set<SimulationSession>();
    public DbSet<Intervention> Interventions => Set<Intervention>();
    public DbSet<VitalSignsRecord> VitalSignRecords => Set<VitalSignsRecord>();
    public DbSet<TimelineEvent> TimelineEvents => Set<TimelineEvent>();
    public DbSet<TeacherObservation> TeacherObservations => Set<TeacherObservation>();
    public DbSet<DebriefReport> DebriefReports => Set<DebriefReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaseScenario>().HasOne((c) => c.Patient);
        modelBuilder.Entity<CaseScenario>().HasOne((c) => c.InitialVitals);

        base.OnModelCreating(modelBuilder);
    }
}