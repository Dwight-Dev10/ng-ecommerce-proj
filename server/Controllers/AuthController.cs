using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ng_ecommerce_api.Constants;
using ng_ecommerce_api.DTOs.Auth;
using ng_ecommerce_api.Models;
using ng_ecommerce_api.Services.Interfaces;

namespace ng_ecommerce_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, Name = dto.Name };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = result.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray()
                        )
            });
        }

        var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.Customer);

        if (!roleResult.Succeeded)
        {
            // Clean up the account if role assignment fails.
            await _userManager.DeleteAsync(user);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { Errors = roleResult.Errors.Select(error => error.Description) }
            );
        }

        return CreatedAtAction(
            nameof(Register),
            new { user.Id, user.Email, user.Name }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            dto.Password,
            lockoutOnFailure: true
        );

        if (!result.Succeeded)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        var accessToken = await _jwtTokenService.CreateToken(user);

        return Ok(
            new
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = 900,
            }
        );
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(
            new
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = User.FindFirstValue(ClaimTypes.Email),
                IsAuthenticated = User.Identity?.IsAuthenticated,
            }
        );
    }
}
