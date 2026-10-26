using Azure.Storage.Blobs;
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

    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts(string name)
    {
        var products = await _productService.GetProductByName(name);
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
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductDto productDto)
    {
        var product = await _productService.CreateProduct(productDto);
      
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
    
    
    // Update
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProduct)
    {
        var  product =  await _productService.UpdateProduct(id, updateProduct);

        if (!product) return NotFound();
        
        return NoContent();
    }
    
    // Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _productService.DeleteProduct(id);
        
        if (!product) return NotFound();
        
        return NoContent();

    }

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _productService.GetProductCount();

        return Ok(count);
    }
    
}