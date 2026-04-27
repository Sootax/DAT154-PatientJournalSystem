using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Shared.Infrastructure.Persistence;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CasesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<CaseScenario>>> GetCases()
    {
        var cases = await _db.CaseScenarios
            .Include((c) => c.Patient)
            .Include((c) => c.InitialVitals)
            .ToListAsync();

        return Ok(cases);
    }

    [HttpPost]
    public async Task<ActionResult<CaseScenario>> CreateCase(CaseScenario caseScenario)
    {
        _db.CaseScenarios.Add(caseScenario);
        await _db.SaveChangesAsync();

        return Ok(caseScenario);
    }

    [HttpPost("{id}/activate")]
    public async Task<ActionResult> ActivateCase(int id)
    {
        var allCases = await _db.CaseScenarios.ToListAsync();

        foreach (var c in allCases)
        {
            c.IsActive = false;
        }

        var selectedCase = allCases.FirstOrDefault((c) => c.CaseScenarioId == id);

        if (selectedCase is null)
        {
            return NotFound();
        }

        selectedCase.IsActive = true;
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("active")]
    public async Task<ActionResult<CaseScenario>> GetActiveCase()
    {
        var activeCase = await _db.CaseScenarios
            .Include((c) => c.Patient)
            .Include((c) => c.InitialVitals)
            .Include((c) => c.Medications)
            .Include((c) => c.Allergies)
            .Include((c) => c.Goals)
            .FirstOrDefaultAsync((c) => c.IsActive);

        if (activeCase is null)
        {
            return NotFound();
        }

        return Ok(activeCase);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CaseScenario>> GetCaseById(int id)
    {
        var caseScenario = await _db.CaseScenarios
            .Include((c) => c.Patient)
            .Include((c) => c.InitialVitals)
            .Include((c) => c.Medications)
            .Include((c) => c.Allergies)
            .Include((c) => c.Goals)
            .FirstOrDefaultAsync((c) => c.CaseScenarioId == id);

        if (caseScenario is null)
        {
            return NotFound();
        }

        return Ok(caseScenario);
    }
}