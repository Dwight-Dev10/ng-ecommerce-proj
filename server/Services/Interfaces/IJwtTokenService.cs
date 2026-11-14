using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Services.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateToken(ApplicationUser user);
}