namespace Shared.Domain.Entities;

public class TeacherObservation
{
    public int TeacherObservationId { get; set; }
    public int SimulationSessionId { get; set; }
    public int TeacherUserId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Note { get; set; } = string.Empty;
}