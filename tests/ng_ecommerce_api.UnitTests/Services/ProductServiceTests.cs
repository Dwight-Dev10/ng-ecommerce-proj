using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.Models;
using ng_ecommerce_api.Services;
using Xunit;

namespace ng_ecommerce_api.UnitTests.Services;

public class ProductServiceTests
{
    // Service unit testing
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
    public async Task SearchForProducts_WhenSearchTermIsEmpty_ReturnsAllProducts()
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
        string search = "";
        var result = await sut.SearchForProducts(search);
        

        // Assert
        
        Assert.Equal(2, result.Count());

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

    [Fact]
    public async Task GetProductByName_WhenProductsExist_ReturnsMatchingProducts()
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
        var result = await sut.GetProductByName("Gaming Mouse");
        // var mathchingProduct = Assert.Single(result);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Gaming Mouse", result.Name);
    }

    [Fact]
    public async Task GetProductByCategory_WhenCategoryExist_ReturnsProducts()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);

        // Act

        var result = await sut.GetProductByCategory("Electronics");

        // Assert
        Assert.Contains(result, p => p.Category == "Electronics");
    }

    [Fact]
    public async Task GetProductByCategory_WhenCategoryDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);

        // Act
        var result = await sut.GetProductByCategory("Toys");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreateProduct_CreateNewProduct_ReturnsNewProduct()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);

        // Act
        var result = await sut.CreateProduct(new DTOs.Products.CreateProductDto
        {
            Name = "Gaming Mouse",
            Description = "A gaming mouse",
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics",
        });

        var savedProduct = await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == result.Id);



        // Assert
        Assert.NotNull(result);
        Assert.NotNull(savedProduct);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Gaming Mouse", result.Name);
        Assert.Equal("A gaming mouse", result.Description);
        Assert.Equal(79.99m, result.Price);
        Assert.Equal("mouse.jpg", result.ImageUrl);
        Assert.Equal("Electronics", result.Category);
        Assert.True(result.InStock);
    }
    [Fact]
    public async Task UpdateProduct_UpdateExistingProduct_ReturnsProductWithNewData()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

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
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.UpdateProduct(product.Id, new DTOs.Products.UpdateProductDto
        {
            Name = "Updated Gaming Mouse",
            Description = "An updated gaming mouse",
            Price = 89.99m,
            ImageUrl = "updated_mouse.jpg",
            Category = "Updated Electronics",
            InStock = false
        });

        // Assert
        Assert.True(result);
        Assert.Equal("Updated Gaming Mouse", product.Name);
        Assert.Equal("An updated gaming mouse", product.Description);
        Assert.Equal(89.99m, product.Price);
        Assert.Equal("updated_mouse.jpg", product.ImageUrl);
        Assert.Equal("Updated Electronics", product.Category);
        Assert.False(product.InStock);
    }
    [Fact]
    public async Task UpdateProduct_ProductDoesNotExist_ReturnsFalse()
    {
         // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);
        var noneExistingId = Guid.NewGuid();

        // Act
        var result = await sut.UpdateProduct(noneExistingId, new DTOs.Products.UpdateProductDto
        {
            Name = "Updated Gaming Mouse"
            });

        // Assert
        Assert.False(result);

    }

    [Fact]
    public async Task DeleteProduct_ProductExists_ReturnsTrue()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

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
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await sut.DeleteProduct(product.Id);

        Assert.True(result);
        Assert.Null(await context.Products.FindAsync(product.Id));
    }
    [Fact]
    public async Task DeleteProduct_ProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        await using var connection = new SqliteConnection("Data Source=:memory");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new AppDbContext(options);

        await context.Database.EnsureCreatedAsync();

        var sut = new ProductService(context);
        var noneExistingId = Guid.NewGuid();

        // Act
        var result = await sut.DeleteProduct(noneExistingId);

        // Assert
        Assert.False(result);
    }


}