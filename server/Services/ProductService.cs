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
    
    public async Task<IEnumerable<ProductDto>> SearchForProducts(string name)
    {
        return await _context.Products
            .Where(p => p.Name.ToLower().Contains(name.ToLower()))
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

    public async Task<ProductDto?> CreateProduct(
        Guid userId,
        CreateProductDto dto)
    {
        var sellerProfile = await _context.SellerProfiles
            .FirstOrDefaultAsync(sp => sp.UserId == userId);

        if (sellerProfile is null)
            return null;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            SellerProfileId = sellerProfile.Id,
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
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            Category = product.Category,
            InStock = product.InStock
        };
    }
    
    public async Task<bool> UpdateProduct(
        Guid userId,
        Guid id,
        UpdateProductDto dto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p =>
                p.Id == id &&
                p.SellerProfile.UserId == userId);

        if (product is null)
            return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;
        product.Category = dto.Category;
        product.InStock = dto.InStock;

        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> DeleteProduct(Guid userId, Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p =>
                p.Id == id &&
                p.SellerProfile.UserId == userId);

        if (product is null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetProductCount()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetMyProducts(Guid userId)
    {
        return await _context.Products
            .Where(p => p.SellerProfile.UserId == userId)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Category = p.Category,
                InStock = p.InStock
            })
            .ToListAsync();
    }

    
}