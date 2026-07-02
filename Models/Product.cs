using System.ComponentModel.DataAnnotations;

namespace ng_ecommerce_api.Models;

public class Product
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool InStock { get; set; }
    public string Category { get; set; } = string.Empty;
}
