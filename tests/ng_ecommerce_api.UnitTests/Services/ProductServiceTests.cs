using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.Models;
using ng_ecommerce_api.Services;
using Xunit;

namespace ng_ecommerce_api.UnitTests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var productId = Guid.NewGuid();

        context.Products.Add(new Product
        {
            Id = productId,
            Name = "Mechanical Keyboard",
            Description = "A mechanical keyboard",
            Price = 89.99m,
            ImageUrl = "keyboard.jpg",
            Category = "Electronics",
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        });

        await context.SaveChangesAsync();

        var sut = new ProductService(context);

        // Act
        var result = await sut.GetProductById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Mechanical Keyboard", result.Name);
        Assert.Equal(89.99m, result.Price);
        Assert.Equal("Electronics", result.Category);
        Assert.True(result.InStock);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);
        var missingId = Guid.NewGuid();

        // Act
        var result = await sut.GetProductById(missingId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchForProducts_WhenProductsExist_ReturnsMatchingProducts()
    {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);

        context.Products.RemoveRange(context.Products);
        await context.SaveChangesAsync();

        var product1 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Gaming Mouse",
            Description = "A gaming mouse",
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics",
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        };

        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mechanical Keyboard",
            Description = "A mechanical keyboard",
            Price = 89.99m,
            ImageUrl = "keyboard.jpg",
            Category = "Electronics",
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        };
        
        context.Products.AddRange(product1,product2);
        await context.SaveChangesAsync();
        
        // Act
        string search = "Gam";
        var result = await sut.SearchForProducts(search);
        

        // Assert
        var matchingProduct = Assert.Single(result);

        Assert.Equal(product1.Id, matchingProduct.Id);
        Assert.Equal("Gaming Mouse", matchingProduct.Name);

    }

    [Fact]
    public async Task GetAllProducts_WhenProductsExist_ReturnsAllProducts()
    {
        // Arrange
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        await using var connection =
            new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        // EF Core creates the database and inserts those five products.
        await context.Database.EnsureCreatedAsync();

        // Remove seeded products
        context.Products.RemoveRange(context.Products);
        await context.SaveChangesAsync();

        var sut = new ProductService(context);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Gaming Mouse",
            Description = "A gaming mouse",
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics",
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        };
        
        var product2 = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mechanical Keyboard",
            Description = "A mechanical keyboard",
            Price = 89.99m,
            ImageUrl = "keyboard.jpg",
            Category = "Electronics",
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        };
        // Add products to the context
        context.Products.AddRange(product, product2);
        await context.SaveChangesAsync();
        
        // Act
        var result = await sut.GetAllProducts();


        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Id == product.Id);

    }

}