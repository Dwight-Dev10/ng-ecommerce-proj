using System;
using Microsoft.AspNetCore.Identity;

namespace ng_ecommerce_api.Models;

public class ApplicationUser : IdentityUser<Guid>
{
//     It gives us the standard Identity properties:
// - Id
// - UserName
// - Email
// - PasswordHash
// - EmailConfirmed
// - PhoneNumber
// - LockoutEnabled
// - AccessFailedCount
    public SellerProfile? SellerProfile { get; set; }
    
    
}
