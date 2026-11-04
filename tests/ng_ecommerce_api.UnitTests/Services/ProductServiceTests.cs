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
}