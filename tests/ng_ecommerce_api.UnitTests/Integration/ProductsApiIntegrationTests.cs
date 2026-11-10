using System;
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.Products;


namespace ng_ecommerce_api.UnitTests.Integration;

public class ProductsApiIntegrationTests :IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    // Initialize the database
    public ProductsApiIntegrationTests(
        CustomWebApplicationFactory factory)
    {
        // Create an HTTP client connected to our test API.
        _client = factory.CreateClient();

        // Get access to the test database.
        using var scope = factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        // Create the database schema if it doesn't exist.
        db.Database.EnsureCreated();
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

        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

}
