using CaseSetup.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Dtos;

namespace CaseSetup.Web.Controllers;

public class CasesController : Controller
{
    private readonly ICaseApiClient _caseApiClient;

    public CasesController(ICaseApiClient caseApiClient)
    {
        _caseApiClient = caseApiClient;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("Username") is null)
        {
            return RedirectToAction("Login", "Account");
        }

        IReadOnlyList<CaseScenarioDto> cases = await _caseApiClient.GetAllCasesAsync();
        return View(cases);
    }

    [HttpPost]
    public async Task<IActionResult> Activate(int id)
    {
        await _caseApiClient.ActivateAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
