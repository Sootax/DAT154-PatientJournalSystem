using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Entities;
using Shared.Infrastructure.Persistence;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObservationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ObservationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult> AddObservation(TeacherObservation observation)
    {
        observation.Timestamp = DateTime.UtcNow;
        _db.TeacherObservations.Add(observation);
        _db.TimelineEvents.Add(new TimelineEvent
        {
            SimulationSessionId = observation.SimulationSessionId,
            Timestamp = observation.Timestamp,
            Description = $"Teacher observation: {observation.Note}",
        });

        await _db.SaveChangesAsync();

        return Ok();
    }
}