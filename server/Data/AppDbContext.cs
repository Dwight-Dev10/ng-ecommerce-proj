using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Name = "Wireless Bluetooth Headphones",
                Description = "High-quality over-ear headphones with noise cancellation.",
                Price = 199.99m,
                ImageUrl = "https://example.com/images/headphones.jpg",
                Rating = 4.5,
                ReviewCount = 120,
                InStock = true,
                Category = "Electronics"
            },
            new Product
            {
                Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                Name = "Mechanical Gaming Keyboard",
                Description = "RGB backlit mechanical keyboard with blue switches.",
                Price = 89.50m,
                ImageUrl = "https://example.com/images/keyboard.jpg",
                Rating = 4.8,
                ReviewCount = 85,
                InStock = true,
                Category = "Accessories"
            },
            new Product
            {
                Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3301"),
                Name = "Ergonomic Office Chair",
                Description = "Comfortable mesh chair with lumbar support.",
                Price = 249.00m,
                ImageUrl = "https://example.com/images/chair.jpg",
                Rating = 4.2,
                ReviewCount = 45,
                InStock = false,
                Category = "Furniture"
            },
            new Product
            {
                Id = Guid.Parse("11112222-3333-4444-5555-666677778888"),
                Name = "Ultra-Wide Monitor",
                Description = "34-inch curved monitor for immersive productivity.",
                Price = 450.00m,
                ImageUrl = "https://example.com/images/monitor.jpg",
                Rating = 4.7,
                ReviewCount = 30,
                InStock = true,
                Category = "Electronics"
            },
            new Product
            {
                Id = Guid.Parse("99998888-7777-6666-5555-444433332222"),
                Name = "Reusable Water Bottle",
                Description = "Stainless steel insulated water bottle, 32oz.",
                Price = 25.00m,
                ImageUrl = "https://example.com/images/bottle.jpg",
                Rating = 4.9,
                ReviewCount = 210,
                InStock = true,
                Category = "Home & Kitchen"
            }
        );
    }

    public DbSet<Product> Products { get; set; } // represents database table
}