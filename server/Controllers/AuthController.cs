using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ng_ecommerce_api.DTOs.Auth;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description);

            return BadRequest(new
            {
                Errors = errors
            });
        }

        return StatusCode(StatusCodes.Status201Created, new
        {
            user.Id,
            user.Email
        });
    }
}