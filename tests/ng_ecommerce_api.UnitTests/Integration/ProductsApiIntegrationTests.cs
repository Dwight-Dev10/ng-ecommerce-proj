using System;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Models;



namespace ng_ecommerce_api.UnitTests.Integration;

public class ProductsApiIntegrationTests : IDisposable
{
    private readonly HttpClient _client;

    private readonly CustomWebApplicationFactory _factory;

    public void Dispose()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
    
    // Initialize the database
    public ProductsApiIntegrationTests()
    {
        _factory = new CustomWebApplicationFactory();
        // Create an HTTP client connected to our test API.
        _client = _factory.CreateClient();

        // Get access to the test database.
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        // Create the database schema if it doesn't exist.
        db.Database.EnsureCreated();
    }

    private async Task<Guid> CreateTestProductAsync(
        CustomWebApplicationFactory factory)
    {
        using var scope = factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test Product",
            Description = "Created for integration testing",
            Price = 49.99m,
            Category = "Electronics",
            ImageUrl = "test-product.jpg",
            Rating = 4.5,
            ReviewCount = 10,
            InStock = true
        };

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return product.Id;
    }

    [Fact]
    public async Task GetProducts_ReturnsOkWithProducts()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content
            .ReadFromJsonAsync<List<ProductDto>>();

        Assert.NotEmpty(products);

        Assert.All(products, product =>
        {
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.False(string.IsNullOrWhiteSpace(product.Name));
            Assert.True(product.Price >= 0);
        });
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsOk()
    {
        // Arrange
        var productId = await CreateTestProductAsync(_factory);

        // Act
        var response = await _client.GetAsync($"/api/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var product = await response.Content
            .ReadFromJsonAsync<ProductDto>();

        Assert.NotNull(product);
        Assert.Equal(productId, product.Id);
        Assert.Equal("Integration Test Product", product.Name);
        Assert.Equal(49.99m, product.Price);
        Assert.Equal("Electronics", product.Category);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAndPersistsProduct()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "Integration Gaming Mouse",
            Description = "A gaming mouse created through an API request",
            Price = 79.99m,
            ImageUrl = "gaming-mouse.jpg",
            Category = "Electronics"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            createProductDto);

        // Assert - HTTP Response
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var createdProduct = await response.Content
            .ReadFromJsonAsync<ProductDto>();

        Assert.NotNull(createdProduct);
        Assert.NotEqual(Guid.Empty, createdProduct.Id);
        Assert.Equal(createProductDto.Name, createdProduct.Name);
        Assert.Equal(createProductDto.Price, createdProduct.Price);
        Assert.Equal(createProductDto.Category, createdProduct.Category);


        // Assert - Database Persistence
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var savedProduct = await db.Products
            .FirstOrDefaultAsync(p => p.Id == createdProduct.Id);

        Assert.NotNull(savedProduct);
        Assert.Equal(createProductDto.Name, savedProduct.Name);
        Assert.Equal(createProductDto.Price, savedProduct.Price);
        Assert.Equal(createProductDto.Description, savedProduct.Description);
        Assert.Equal(createProductDto.ImageUrl, savedProduct.ImageUrl);
        Assert.Equal(createProductDto.Category, savedProduct.Category);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductExists_ReturnsNoContent()
    {
        // Arrange
        var productId = await CreateTestProductAsync(_factory);

        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Gaming Mouse",
            Description = "Updated description",
            Price = 99.99m,
            ImageUrl = "updated-mouse.jpg",
            Category = "Gaming",
            InStock = false
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/products/{productId}",
            updateProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var updatedProduct = await db.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Gaming Mouse", updatedProduct.Name);
        Assert.Equal("Updated description", updatedProduct.Description);
        Assert.Equal(99.99m, updatedProduct.Price);
        Assert.Equal("updated-mouse.jpg", updatedProduct.ImageUrl);
        Assert.Equal("Gaming", updatedProduct.Category);
        Assert.False(updatedProduct.InStock);
    }


    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Gaming Mouse",
            Description = "Updated description",
            Price = 99.99m,
            ImageUrl = "updated-mouse.jpg",
            Category = "Gaming",
            InStock = false
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/products/{productId}",
            updateProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ReturnsNoContent()
    {
        // Arrange
        var productId = await CreateTestProductAsync(_factory);

        // Act
        var response = await _client.DeleteAsync(
            $"/api/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var deletedProduct = await db.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/products/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidPrice_ReturnsBadRequest()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "Gaming Mouse",
            Description = "A gaming mouse",
            Price = -50m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            createProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorResponse = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(errorResponse);
        Assert.Contains("Price", errorResponse.Errors.Keys);

        using var scope = _factory.Services.CreateScope();

        // Verify invalid requests don't reach the database
        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var savedProduct = await db.Products
            .FirstOrDefaultAsync(p => p.Name == createProductDto.Name);

        Assert.Null(savedProduct);

    }


    [Fact]
    public async Task CreateProduct_WithMissingName_ReturnsBadRequest()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "",
            Description = "A gaming mouse",
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            createProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorResponse = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(errorResponse);
        Assert.Contains("Name", errorResponse.Errors.Keys);
    }

    [Fact]
    public async Task CreateProduct_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = new string('A', 101),
            Description = "A gaming mouse",
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            createProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorResponse = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(errorResponse);
        Assert.Contains("Name", errorResponse.Errors.Keys);
    }

    [Fact]
    public async Task CreateProduct_WithDescriptionExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "Gaming Mouse",
            Description = new string('D', 501),
            Price = 79.99m,
            ImageUrl = "mouse.jpg",
            Category = "Electronics"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            createProductDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorResponse = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(errorResponse);
        Assert.Contains("Description", errorResponse.Errors.Keys);
    }

    [Fact]
    public async Task CreateProduct_WithMalformedJson_ReturnsBadRequest()
    {
        // Arrange
        var content = new StringContent(
            "{ \"name\": \"Gaming Mouse\", \"price\": }",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/products", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchProducts_WhenMatchesExist_ReturnsMatchingProducts()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var newProducts = new List<Product>
        {
            new Product
            {
                Name = "Gaming Mouse",
                Description = "A gaming mouse",
                Price = 79.99m,
                ImageUrl = "mouse.jpg",
                Category = "Electronics"
            },
            new Product
            {
                Name = "Gaming Keyboard",
                Description = "A gaming keyboard",
                Price = 129.99m,
                ImageUrl = "keyboard.jpg",
                Category = "Electronics"
            },
            new Product
            {
                Name = "Zebra Chair",
                Description = "An ergonomic office chair",
                Price = 199.99m,
                ImageUrl = "chair.jpg",
                Category = "Furniture"
            }
        };
        
        db.Products.RemoveRange(db.Products);
        await db.SaveChangesAsync();    

        db.Products.AddRange(newProducts);
        await db.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/products/search/Gaming");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
        Assert.All(products, product =>
        {
            Assert.Contains("Gaming", product.Name);
        });
    }

    [Fact]
    public async Task GetProductsByCategory_WhenProductsExist_ReturnsMatchingProducts()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        var newProducts = new List<Product>
        {
            new Product
            {
                Name = "Lamp",
                Description = "A stylish lamp",
                Price = 79.99m,
                ImageUrl = "lamp.jpg",
                Category = "Furniture"
            },
            new Product
            {
                Name = "Gaming Keyboard",
                Description = "A gaming keyboard",
                Price = 129.99m,
                ImageUrl = "keyboard.jpg",
                Category = "Electronics"
            },
            new Product
            {
                Name = "Zebra Couch",
                Description = "An ergonomic living room couch",
                Price = 199.99m,
                ImageUrl = "couch.jpg",
                Category = "Furniture"
            }
        };
        
        db.Products.RemoveRange(db.Products);
        await db.SaveChangesAsync();    

        db.Products.AddRange(newProducts);
        await db.SaveChangesAsync();


        // Act
        var response = await _client.GetAsync("/api/products/category/Furniture");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
        Assert.All(products, product =>
        {
            Assert.Equal("Furniture", product.Category);
        });
    }

    [Fact]
    public async Task GetProductsByCategory_WhenNoProductsExist_ReturnsNotFound()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        db.Products.RemoveRange(db.Products);
        await db.SaveChangesAsync();    


        // Act
        var response = await _client.GetAsync("/api/products/category/NonExistentCategory");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
