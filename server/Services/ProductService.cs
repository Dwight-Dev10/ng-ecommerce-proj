using Microsoft.EntityFrameworkCore;
using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        return await _context.Products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Category = p.Category,
            InStock = p.InStock
        }).ToListAsync();
    }

    public async Task<ProductDto?> GetProductById(Guid id)
    {
        return await _context.Products.Where(p => p.Id == id)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                InStock = p.InStock
            }).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<ProductDto>> SearchProductsByName(string name)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(name))
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Category = p.Category
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetProductByName(string name)
    {
        return await _context.Products
            .Where(p => p.Name == name)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Category = p.Category,
                InStock = p.InStock
            }).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductByCategory(string category)
    {
        return await _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Category = p.Category
            }).ToListAsync();
    }

    public async Task<ProductDto> CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            Rating = 0,
            ReviewCount = 0,
            InStock = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            // Description = product.Description,
            Category = product.Category,
            InStock = product.InStock
        };
    }
    
    public async Task<bool> UpdateProduct(Guid id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;
        product.Category = dto.Category;
        product.InStock = dto.InStock;

        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> DeleteProduct(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetProductCount()
    {
        return await _context.Products.CountAsync();
    }

    
}