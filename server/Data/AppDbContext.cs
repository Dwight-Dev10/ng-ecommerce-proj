using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var demoSellerUserId = Guid.Parse(
            "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var demoSellerProfileId = Guid.Parse(
            "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = demoSellerUserId,
                UserName = "demo-store-owner@seed.local",
                NormalizedUserName = "DEMO-STORE-OWNER@SEED.LOCAL",
                Email = "demo-store-owner@seed.local",
                NormalizedEmail = "DEMO-STORE-OWNER@SEED.LOCAL",
                EmailConfirmed = false,
                SecurityStamp = "DEMO-STORE-OWNER-SECURITY-STAMP",
                ConcurrencyStamp = "DEMO-STORE-OWNER-CONCURRENCY-STAMP",
                LockoutEnabled = true
            }
        );

        modelBuilder.Entity<SellerProfile>().HasData(
            new SellerProfile
            {
                Id = demoSellerProfileId,
                UserId = demoSellerUserId,
                StoreName = "Demo Store",
                Description = "Default store for demonstration products.",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<SellerProfile>()
            .HasMany<Product>()
            .WithOne(product => product.SellerProfile)
            .HasForeignKey(product => product.SellerProfileId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict prevents deleting a seller profile while products still reference it
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                SellerProfileId = demoSellerProfileId,
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
                SellerProfileId = demoSellerProfileId,
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
                SellerProfileId = demoSellerProfileId,
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
                SellerProfileId = demoSellerProfileId,
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
                SellerProfileId = demoSellerProfileId,
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

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(user => user.SellerProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<SellerProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Product> Products { get; set; } // represents database table
    public DbSet<SellerProfile> SellerProfiles => Set<SellerProfile>();
}