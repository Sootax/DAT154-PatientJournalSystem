using Shared.Contracts.Requests;

namespace TeacherAssessment.Services;

public interface IObservationApiClient
{
    Task AddObservationAsync(AddObservationRequest request);
}