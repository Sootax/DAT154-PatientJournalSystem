using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Services;
using Shared.Domain.Entities;
using Shared.Infrastructure.Persistence;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly SimulationEngine _simulationEngine;
    private readonly DebriefService _debriefService;

    public SimulationController(AppDbContext db, SimulationEngine simulationEngine, DebriefService debriefService)
    {
        _db = db;
        _simulationEngine = simulationEngine;
        _debriefService = debriefService;
    }

    [HttpPost("start/{caseId}")]
    public async Task<ActionResult<SimulationSession>> StartSession(int caseId)
    {
        var caseScenario = await _db.CaseScenarios
            .Include((c) => c.InitialVitals)
            .FirstOrDefaultAsync((c) => c.CaseScenarioId == caseId);

        if (caseScenario is null || caseScenario.InitialVitals is null)
        {
            return NotFound();
        }

        var session = new SimulationSession
        {
            CaseScenarioId = caseId,
            StartedAt = DateTime.UtcNow,
            Status = "Running"
        };

        _db.SimulationSessions.Add(session);
        await _db.SaveChangesAsync();

        var firstVitals = new VitalSignsRecord
        {
            SimulationSessionId = session.SimulationSessionId,
            Timestamp = DateTime.UtcNow,
            SystolicBp = caseScenario.InitialVitals.SystolicBp,
            DiastolicBp = caseScenario.InitialVitals.DiastolicBp,
            HeartRate = caseScenario.InitialVitals.HeartRate,
            RespiratoryRate = caseScenario.InitialVitals.RespiratoryRate,
            OxygenSaturation = caseScenario.InitialVitals.OxygenSaturation,
            TemperatureCelsius = caseScenario.InitialVitals.TemperatureCelsius,
        };

        _db.VitalSignRecords.Add(firstVitals);
        _db.TimelineEvents.Add(new TimelineEvent
        {
            SimulationSessionId = session.SimulationSessionId,
            Timestamp = DateTime.UtcNow,
            Description = "Simulation session started.",
        });

        await _db.SaveChangesAsync();

        return Ok(session);
    }

    [HttpGet("{sessionId}")]
    public async Task<ActionResult<SimulationSession>> GetSession(int sessionId) 
    {
        var session = await _db.SimulationSessions
            .Include((session) => session.Interventions)
            .Include((session) => session.VitalHistory)
            .Include((session) => session.TimelineEvents)
            .Include((session) => session.Observations)
            .FirstOrDefaultAsync((session) => session.SimulationSessionId == sessionId);

        if (session is null)
        {
            return NotFound();
        }

        return Ok(session);
    }

    [HttpPost("{sessionId}/intervention")]
    public async Task<ActionResult> RegisterIntervention(int sessionId, Intervention intervention)
    {
        var session = await _db.SimulationSessions.FirstOrDefaultAsync((session) => session.SimulationSessionId == sessionId);

        if (session is null)
        {
            return NotFound();
        }

        intervention.SimulationSessionId = sessionId;
        intervention.Timestamp = DateTime.UtcNow;

        _db.Interventions.Add(intervention);

        var currentVitals = await _db.VitalSignRecords
            .Where((vital) => vital.SimulationSessionId == sessionId)
            .OrderByDescending((vital) => vital.Timestamp)
            .FirstOrDefaultAsync();

        if (currentVitals is not null)
        {
            var updatedVitals = _simulationEngine.ApplyInterventionEffect(currentVitals, intervention);

            _db.VitalSignRecords.Add(updatedVitals);
        }

        _db.TimelineEvents.Add(new TimelineEvent
        {
            SimulationSessionId = sessionId,
            Timestamp = DateTime.UtcNow,
            Description = $"Intervention registered: {intervention.Type} " +
            $"{intervention.DrugName ?? intervention.Description}",
        });

        await _db.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("{sessionId}/end")]
    public async Task<ActionResult<DebriefReport>> EndSession(int sessionId)
    {
        var session = await _db.SimulationSessions
            .Include((session) => session.Interventions)
            .Include((session) => session.Observations)
            .FirstOrDefaultAsync((session) => session.SimulationSessionId == sessionId);

        if (session is null)
        {
            return NotFound();
        }

        session.EndedAt = DateTime.UtcNow;
        session.Status = "Ended";

        var report = _debriefService.GenerateReport(session);
        _db.DebriefReports.Add(report);

        _db.TimelineEvents.Add(new TimelineEvent
        {
            SimulationSessionId = sessionId,
            Timestamp = DateTime.UtcNow,
            Description = "Simulation session ended.",
        });

        await _db.SaveChangesAsync();

        return Ok(report);
    }
}