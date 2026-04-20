using CaseSetup.Web.Models;
using CaseSetup.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace CaseSetup.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthApiClient _authApiClient;

    public AccountController(IAuthApiClient authApiClient)
    {
        this._authApiClient = authApiClient;
    }

    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _authApiClient.LoginAsync(new LoginRequest
        {
            Username = model.Username,
            Password = model.Password
        });

        if (user is null)
        {
            model.ErrorMessage = "Invalid username or password.";
            return View(model);
        }

        HttpContext.Session.SetInt32("UserId", user.UserId);
        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Role", user.Role);

        return RedirectToAction("Index", "Cases");
    }
}
