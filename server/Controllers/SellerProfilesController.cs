using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.SellerProfiles;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SellerProfilesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellerProfilesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSellerProfile(
            CreateSellerProfileDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var existingProfile = await _context.SellerProfiles
                .AnyAsync(profile => profile.UserId == userId);

            if (existingProfile)
            {
                return Conflict(new
                {
                    Message = "You already have a seller profile."
                });
            }

            var sellerProfile = new SellerProfile
            {
                UserId = userId,
                StoreName = dto.StoreName,
                Description = dto.Description
            };

            _context.SellerProfiles.Add(sellerProfile);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMySellerProfile),
                new
                {
                    id = sellerProfile.Id
                },
                sellerProfile
            );
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMySellerProfile()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var sellerProfile = await _context.SellerProfiles
                .FirstOrDefaultAsync(profile => profile.UserId == userId);

            if (sellerProfile is null)
            {
                return NotFound(new
                {
                    Message = "You do not have a seller profile yet."
                });
            }

            return Ok(sellerProfile);
        }
    }


}
