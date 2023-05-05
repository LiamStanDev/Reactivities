using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    // Why not using mediator?
    // Because we want seperate identity logic from business logic
    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        // identity need to combine the user data and password to make hash
        // , then check the password (hashed) in database.
        bool result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
        {
            return Unauthorized();
        }

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Image = null,
            Token = "this will be a token",
            UserName = user.UserName,
        };
    }
}
