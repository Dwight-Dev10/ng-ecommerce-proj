using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Services;

namespace ng_ecommerce_api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    // Get product by Name
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetProductsByName(string name)
    {
        var product = await _productService.GetProductByName(name);

        if (product == null)
        {
            return NotFound($"No products found with name: {name}");
        }
        return Ok(product);
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchProducts(string name)
    {
        var products = await _productService.SearchForProducts(name);
        return Ok(products);
    }

    // GET products by Category
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category)
    {
        var productsInCategory = await _productService.GetProductByCategory(category);
        if (productsInCategory.Any())
        {
            return Ok(productsInCategory);
        }
        return NotFound($"No products found in category: {category}");
    }

    //Create
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        var userId = GetCurrentUserId();

        var product = await _productService.CreateProduct(userId, productDto);

        if (product is null)
        {
            return BadRequest(
                new { Message = "You must create a seller profile before listing products." }
            );
        }

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    // Update
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductDto updateProduct
    )
    {
        var userId = GetCurrentUserId();

        var updated = await _productService.UpdateProduct(userId, id, updateProduct);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var userId = GetCurrentUserId();

        var deleted = await _productService.DeleteProduct(userId, id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _productService.GetProductCount();

        return Ok(count);
    }

    // private helper
    private Guid GetCurrentUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [Authorize]
    [HttpGet("my-products")]
    public async Task<IActionResult> GetMyProducts()
    {
        var userId = GetCurrentUserId();

        var products = await _productService.GetMyProducts(userId);

        return Ok(products);
    }
}
