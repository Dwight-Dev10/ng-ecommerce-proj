using System;

namespace ng_ecommerce_api.Models;

public class SellerProfile
{
    // Unique seller profile identifier
    public Guid Id { get; set; }

    // Links the profile to an Identity user
    public Guid UserId { get; set; }

    // Seller's public store name
    public string StoreName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ApplicationUser User { get; set; } = null!;
}
