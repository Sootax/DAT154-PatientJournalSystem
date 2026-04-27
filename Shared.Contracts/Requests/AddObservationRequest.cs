namespace Shared.Contracts.Requests;

public class AddObservationRequest
{
    public int SimulationSessionId { get; set; }
    public int TeacherUserId { get; set; }
    public string Note { get; set; } = string.Empty;
}