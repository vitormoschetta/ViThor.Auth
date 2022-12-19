using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViThor.Auth.Sample.Models;
using ViThor.Auth.Services.User;

namespace ViThor.LoggingFilter.Auth.Sample.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService<User> _userService;

    public UsersController(IUserService<User> userService)
    {
        _userService = userService;
    }


    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        return Ok(await _userService.Get());
    }


    [HttpGet("{name}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<User>> GetByName(string name)
    {
        var user = await _userService.GetByUsername(name);
        if (user == null)
            return NotFound();

        return Ok(user);
    }


    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var username = User.Identity?.Name;

        if (username == null)
            return Unauthorized(new { message = "Invalid token: username not found" });

        var user = await _userService.GetByUsername(username);
        if (user == null)
        {
            user = await _userService.GetByEmail(username);
            if (user == null)
                return Unauthorized(new { message = "Invalid token: user not found" });
        }

        return Ok(user);
    }
}
