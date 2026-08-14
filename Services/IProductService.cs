using ng_ecommerce_api.DTOs.Products;

namespace ng_ecommerce_api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts();

    Task<ProductDto?> GetProductById(Guid id);
    Task<ProductDto?> GetProductByName(string name);
    Task<IEnumerable<ProductDto>> SearchProductsByName(string name);
    Task<IEnumerable<ProductDto>> GetProductByCategory(string category);
    
    Task<ProductDto> CreateProduct(CreateProductDto dto);

    Task<bool> UpdateProduct(Guid id, UpdateProductDto dto);

    Task<bool> DeleteProduct(Guid id);

    Task<int> GetProductCount();
}