using Microsoft.AspNetCore.Components;
using TeacherAssessment.Services;

namespace TeacherAssessment.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private TeacherSessionState SessionState { get; set; } = default!;
}