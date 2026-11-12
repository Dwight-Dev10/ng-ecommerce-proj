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
        var productId = Guid.NewGuid();
        _mockProductService.Setup(r => r.GetProductById(productId)).ReturnsAsync((ProductDto?)null);

        var result = await _sut.GetProductById(productId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result);

        Assert.Equal(404, notFoundResult.StatusCode);
    
        _mockProductService.Verify(
            r => r.GetProductById(productId),
            Times.Once
        );
    }

    //GetProductsByName
    [Fact]
    public async Task GetProductsByName_WhenProductExists_ReturnsOk()
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

        _mockProductService.Setup(r => r.GetProductByName(product.Name)).ReturnsAsync(product);

        var result = await _sut.GetProductsByName(product.Name);

        // verifies HTTP 200
        var okResult = Assert.IsType<OkObjectResult>(result);

        // verify that the correct product was returned.
        var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal(product.Id, returnedProduct.Id);
        Assert.Equal(product.Name, returnedProduct.Name);
        Assert.Equal(product.Price, returnedProduct.Price);

        // verify that the service was called exactly once
        _mockProductService.Verify(
            r => r.GetProductByName(product.Name),
            Times.Once
        );
    }

    [Fact]
    public async Task GetProductsByName_WhenProductDoesNotExist_ReturnsNotFound()
    {
        _mockProductService.Setup(r => r.GetProductByName(It.IsAny<string>())).ReturnsAsync((ProductDto?)null);

        var result = await _sut.GetProductsByName("NonExistentProduct");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("No products found with name: NonExistentProduct",
            notFoundResult.Value
        );
    
        _mockProductService.Verify(
            r => r.GetProductByName(It.IsAny<string>()),
            Times.Once
        );
    }

    [Fact]
    public async Task SearchProducts_WhenMatchingProductsExist_ReturnsOkWithProducts()
    {
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Game Console",
                Description = "Description 1",
                Price = 10.0m,
                Category = "Category 1",
                ImageUrl = "http://example.com/image1.jpg",
                InStock = true
            },
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Game chair",
                Description = "Description 2",
                Price = 20.0m,
                Category = "Category 2",
                ImageUrl = "http://example.com/image2.jpg",
                InStock = false
            },
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Car",
                Description = "I am number 3",
                Price = 30.0m,
                Category = "Category 3",
                ImageUrl = "http://example.com/image2.jpg",
                InStock = true
        }};

        _mockProductService.Setup(r => r.SearchForProducts("Game")).ReturnsAsync(products.Where(p => p.Name.Contains("Game")));

        var result = await _sut.SearchProducts("Game");

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);

        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value).ToList();
        _mockProductService.Verify(
            r => r.SearchForProducts("Game"),
            Times.Once
        );

        Assert.Equal(2, returnedProducts.Count);
        Assert.Contains(returnedProducts, p => p.Name == "Game Console");
        Assert.Contains(returnedProducts, p => p.Name == "Game chair");
    }

    [Fact]
    public async Task SearchProducts_WhenNoMatchingProductsExist_ReturnsOkWithEmptyCollection()
    {
        _mockProductService.Setup(r => r.SearchForProducts("Lamp")).ReturnsAsync(new List<ProductDto>());

        var result = await _sut.SearchProducts("Lamp");

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);

        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value).ToList();

        _mockProductService.Verify(
            r => r.SearchForProducts("Lamp"),
            Times.Once
        );     
        Assert.Empty(returnedProducts);

    }

    [Fact]
    public async Task GetProductsByCategory_WhenProductsExist_ReturnsOk()
    {
        var products = new List<ProductDto>
        {
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Game console",
                Description = "Electronics",
                Price = 20.0m,
                Category = "Electronics",
                ImageUrl = "http://example.com/image2.jpg",
                InStock = false
            },
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Car",
                Description = "I am number 3",
                Price = 30.0m,
                Category = "Electronics",
                ImageUrl = "http://example.com/image2.jpg",
                InStock = true
        }};

        _mockProductService.Setup(r => r.GetProductByCategory("Electronics")).ReturnsAsync(products);

        var result = await _sut.GetProductsByCategory("Electronics");

        // var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        var okResult = Assert.IsType<OkObjectResult>(result);


        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value).ToList();

        _mockProductService.Verify(
            r => r.GetProductByCategory("Electronics"),
            Times.Once
        );     

        Assert.Equal(2, returnedProducts.Count);
        Assert.Contains(returnedProducts, p => p.Name == "Game console");
        Assert.Contains(returnedProducts, p => p.Name == "Car");

    }

    [Fact]
    public async Task GetProductsByCategory_WhenNoProductsExist_ReturnsNotFound()
    {
        _mockProductService.Setup(r => r.GetProductByCategory("Electronics")).ReturnsAsync(new List<ProductDto>());

        var result = await _sut.GetProductsByCategory("Electronics");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);


        _mockProductService.Verify(
            r => r.GetProductByCategory("Electronics"),
            Times.Once
        );     

        Assert.Equal("No products found in category: Electronics", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateProduct_WhenValidProduct_ReturnsCreatedAtAction()
    {
        var createProductDto = new CreateProductDto
        {
            Name = "New Product",
            Description = "New Product Description Added",
            Price = 79.99m,
            Category = "Home & Kitchen",
            ImageUrl = "http://example.com/newproduct.jpg",
        };

        var createdProduct = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            ImageUrl = createProductDto.ImageUrl,
            Category = createProductDto.Category,
            InStock = true
        };

        _mockProductService
            .Setup(r => r.CreateProduct(createProductDto))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _sut.CreateProduct(createProductDto);


        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(nameof(_sut.GetProductById), createdResult.ActionName);
        Assert.Equal(createdProduct.Id, createdResult.RouteValues["id"]);

        var returnedProduct = Assert.IsType<ProductDto>(createdResult.Value);

        _mockProductService.Verify(
            r => r.CreateProduct(createProductDto),
            Times.Once
        );

        Assert.Equal(createdProduct.Id, returnedProduct.Id);
        Assert.Equal(createdProduct.Name, returnedProduct.Name);
        Assert.Equal(createdProduct.Price, returnedProduct.Price);
        Assert.Equal(createdProduct.Description, returnedProduct.Description);
        Assert.Equal(createdProduct.Category, returnedProduct.Category);
        Assert.Equal(createdProduct.ImageUrl, returnedProduct.ImageUrl);
        Assert.Equal(createdProduct.InStock, returnedProduct.InStock);
    }

    [Fact]

    public async Task UpdateProduct_WhenProductExists_ReturnsNoContent()
    {
        var productId = Guid.NewGuid();
        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Product",
            Description = "Updated Product Description",
            Price = 99.99m,
            Category = "Electronics",
            ImageUrl = "http://example.com/updatedproduct.jpg",
            InStock = true
        };

        _mockProductService.Setup(r => r.UpdateProduct(productId, updateProductDto)).ReturnsAsync(true);

        var result = await _sut.UpdateProduct(productId, updateProductDto);

        Assert.IsType<NoContentResult>(result);

        _mockProductService.Verify(
            r => r.UpdateProduct(productId, updateProductDto),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDto();

        _mockProductService.Setup(r => r.UpdateProduct(productId, updateDto)).ReturnsAsync(false);

        var result = await _sut.UpdateProduct(productId, updateDto);

        Assert.IsType<NotFoundResult>(result);

        _mockProductService.Verify(
            r => r.UpdateProduct(
                productId, updateDto),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ReturnsNoContent()
    {
        var productId = Guid.NewGuid();
        
        _mockProductService.Setup(r => r.DeleteProduct(productId)).ReturnsAsync(true);

        var result = await _sut.DeleteProduct(productId);

        Assert.IsType<NoContentResult>(result);

        _mockProductService.Verify(
            r => r.DeleteProduct(productId),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var productId = Guid.NewGuid();
        _mockProductService.Setup( r => r.DeleteProduct(productId)).ReturnsAsync(false);

        var result = await _sut.DeleteProduct(productId);
        Assert.IsType<NotFoundResult>(result);

        _mockProductService.Verify(
            r => r.DeleteProduct(productId),
            Times.Once
        );
    }

}
