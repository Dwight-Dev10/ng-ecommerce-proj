using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ng_ecommerce_api.Controller;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Services;
using Xunit;

namespace ng_ecommerce_api.UnitTests.Controller;

public class ProductsControllerTests
{
    // Testing Controller (Behavior) with Mocking the Service Layer
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _sut;// The System Under Test — your actual controller

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>(); //	Creates a mock implementation of your interface

        _sut = new ProductsController(_mockProductService.Object); // Gives the controller the mock implementation
    }

    [Fact]
    public async Task GetProducts_WhenProductsExist_ReturnsOkWithProducts()
    {
        // Arrange

        // Create two ProductDto objects.
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Product 1",
                Description = "Description 1",
                Price = 10.0m,
                Category = "Category 1",
                ImageUrl = "http://example.com/image1.jpg",
                InStock = true
            },
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Product 2",
                Description = "Description 2",
                Price = 20.0m,
                Category = "Category 2",
                ImageUrl = "http://example.com/image2.jpg",
                InStock = false
        }};

        // Configure the mock to return those products
        // when GetAllProducts() is called.
        _mockProductService.Setup(r => r.GetAllProducts()).ReturnsAsync(products);

        // Act

        // Call _sut.GetProducts()
        var result = await _sut.GetProducts();

        // Assert

        // Verify the response is OkObjectResult.
        var okResults = Assert.IsType<OkObjectResult>(result); // returns HTTP 200 OK.

        // Verify the returned collection contains two products.
        var returnProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResults.Value).ToList();
        _mockProductService.Verify(
            r => r.GetAllProducts(),
            Times.Once
        );

        Assert.Equal(2, returnProducts.Count());
        Assert.Equal("Product 1", returnProducts[0].Name);
        Assert.Equal("Product 2", returnProducts[1].Name);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsOk()
    {
        // Arrange
        var product = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Description = "Description 1",
            Price = 10.0m,
            Category = "Category 1",
            ImageUrl = "http://example.com/image1.jpg",
            InStock = true
        };
        
        _mockProductService.Setup(r => r.GetProductById(product.Id)).ReturnsAsync(product);

        var result = await _sut.GetProductById(product.Id);

        // verifies HTTP 200
        var okResult = Assert.IsType<OkObjectResult>(result);

        // verify that the correct product was returned.
        var returnedProducts = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(product.Id, returnedProducts.Id);
        Assert.Equal(product.Name, returnedProducts.Name);
        Assert.Equal(product.Price, returnedProducts.Price);

        // verify that the service was called exactly once
        _mockProductService.Verify(
            r => r.GetProductById(product.Id),
            Times.Once
        );
    }

    //GetProductById
    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // var product = new ProductDto
        // {
        //     Id = Guid.NewGuid(),
        //     Name = "Product 1",
        //     Description = "Description 1",
        //     Price = 10.0m,
        //     Category = "Category 1",
        //     ImageUrl = "http://example.com/image1.jpg",
        //     InStock = true
        // };
        _mockProductService.Setup(r => r.GetProductById(It.IsAny<Guid>())).ReturnsAsync((ProductDto?)null);

        var result = await _sut.GetProductById(Guid.NewGuid());

        var notFoundResult = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, notFoundResult.StatusCode);

        _mockProductService.Verify(
            r => r.GetProductById(It.IsAny<Guid>()),
            Times.Once
        );
    }

    //GetProductsByName

}
