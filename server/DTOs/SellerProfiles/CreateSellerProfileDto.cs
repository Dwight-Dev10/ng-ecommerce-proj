using System;
using System.ComponentModel.DataAnnotations;

namespace ng_ecommerce_api.DTOs.SellerProfiles;

public class CreateSellerProfileDto
{
    [Required]
    [MaxLength(100)]
    public string StoreName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
