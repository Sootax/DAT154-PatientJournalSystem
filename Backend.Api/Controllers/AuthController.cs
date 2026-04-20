using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Persistence;
using Shared.Contracts.Requests;
using Shared.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        this._db = db;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest loginRequest)
    {
        var user = await _db.Users.FirstOrDefaultAsync((user) => user.Username == loginRequest.Username);

        if (user is null)
        {
            return Unauthorized();
        }

        if (user.PasswordHash != loginRequest.Password)
        {
            return Unauthorized();
        }

        var userDto = new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Role = user.Role
        };

        return Ok(userDto);
    }
}
