using System.ComponentModel.DataAnnotations;

namespace ng_ecommerce_api.DTOs.Products;

public class CreateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;
}