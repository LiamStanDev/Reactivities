using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;

    // Why not using mediator?
    // Because we want seperate identity logic from business logic
    public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous] // because login no need authentication
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

        return CreateUserObject(user);
    }

    [AllowAnonymous] // because login no need authentication
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
        {
            return BadRequest("Username is already taken");
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            return BadRequest("Email is already taken");
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.UserName
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreateUserObject(user);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        // User class is in ControllerBase (which do the job about getting user from token)
        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        return CreateUserObject(user);
    }

    private UserDto CreateUserObject(AppUser user)
    {
        return new UserDto
        {
            DisplayName = user.DisplayName,
            Image = null,
            Token = _tokenService.CreateToken(user),
            UserName = user.UserName
        };
    }
}
