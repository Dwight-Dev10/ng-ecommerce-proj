using ng_ecommerce_api.Data;
using ng_ecommerce_api.DTOs.Products;
using ng_ecommerce_api.Models;

namespace ng_ecommerce_api.Services;

public class ProductService : IProductService
{
    // private static readonly List<Product> Products = new();
    private readonly AppDbContext _context;
    
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<ProductDto> GetAllProducts()
    {
        return Products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Category = p.Category,
            InStock = p.InStock
        });
    }

    public ProductDto? GetProductById(Guid id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            InStock = product.InStock
        };
    }
    
    public async Task<List<ProductDto>> SearchProductByNameAsync(string name)
    {
        var results = await _context.Products
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

        return results;
    }

    public ProductDto? GetProductByName(string name)
    {
        var product = Products.FirstOrDefault(name => name.Equals(name));
        
        if (product == null) return null;
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            InStock = product.InStock
        };
    }

    public List<ProductDto> GetProductByCategory(string category)
    {
        // var product = Products.FirstOrDefault(p => p.Category == category);
        

        var product = Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            }).ToList();

        return product;

    }

    public ProductDto CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            Category = dto.Category,
            InStock = true
        };

        Products.Add(product);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            InStock = product.InStock
        };
    }
    
    public bool UpdateProduct(Guid id, UpdateProductDto dto)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null) return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;
        product.Category = dto.Category;
        product.InStock = dto.InStock;

        return true;
    }
    
    public bool DeleteProduct(Guid id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null) return false;

        Products.Remove(product);

        return true;
    }
}