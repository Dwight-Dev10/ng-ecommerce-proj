using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Models;
using ng_ecommerce_api.Services;

namespace ng_ecommerce_api.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly AppDbContext _context;
    
    
    public ProductsController(IProductService productService,
        AppDbContext context)
    {
        // _productService = productService;
        _context = context;
    }

    // private static readonly List<Product> Products;
    
    // GET
    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
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
        var productsWithName = await _context.Products
            .Where(p => p.Name.ToLower() == name.ToLower()).ToListAsync();

        if (!productsWithName.Any())
        {
            return NotFound($"No products found with name: {name}");
        }
        return Ok(productsWithName);
    }

    // GET products by Category
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category)
    {

        var productsInCategory = await _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .ToListAsync();
        if (productsInCategory.Count() > 0)
        {
            return Ok(productsInCategory);
        }
        return NotFound($"No products found in category: {category}");
    }

    //Create
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            ImageUrl = productDto.ImageUrl,
            Category = productDto.Category,
            
            Rating = 0,
            ReviewCount = 0,
            InStock = true,

        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
    
    
    // Update
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProduct)
    {
        var  product =  await _context.Products.FindAsync(id);

        if (product == null) return NotFound();
        
        product.Name = updateProduct.Name;
        product.Description = updateProduct.Description;
        product.Price = updateProduct.Price;
        product.ImageUrl = updateProduct.ImageUrl;
        product.InStock = updateProduct.InStock;
        product.Category = updateProduct.Category;
        
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    // Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var  product = await _context.Products.FindAsync(id);
        
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        
        await _context.SaveChangesAsync();

        return NoContent();

    }


    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var count = await _context.Products.CountAsync();

        return Ok(count);
    }
    // [HttpPost("seed")]
    // public async Task<IActionResult> Seed()
    // {
    //     if (await _context.Products.AnyAsync())
    //         return BadRequest("Database already seeded");
    //
    //     await _context.Products.AddRangeAsync(Products);
    //
    //     await _context.SaveChangesAsync();
    //
    //     return Ok("Products added");
    // }
    //
    
    
    
    
}