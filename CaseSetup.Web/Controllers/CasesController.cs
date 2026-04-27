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

    public async Task<IActionResult> Details(int id)
    {
        if (HttpContext.Session.GetString("Username") is null)
        {
            return RedirectToAction("Login", "Account");
        }

        var caseScenario = await _caseApiClient.GetCaseByIdAsync(id);

        if (caseScenario is null)
        {
            return NotFound();
        }

        return View(caseScenario);
    }
}
