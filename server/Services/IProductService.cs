using ng_ecommerce_api.DTOs.Products;

namespace ng_ecommerce_api.Services;

public interface IProductService
{
    // Public product browsing
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<ProductDto?> GetProductById(Guid id);
    Task<ProductDto?> GetProductByName(string name);
    Task<IEnumerable<ProductDto>> SearchForProducts(string name);
    Task<IEnumerable<ProductDto>> GetProductByCategory(string category);
    Task<int> GetProductCount();

    // Seller product management
    Task<IEnumerable<ProductDto>> GetMyProducts(Guid userId);

    Task<ProductDto?> CreateProduct(
        Guid userId,
        CreateProductDto dto);

    Task<bool> UpdateProduct(
        Guid userId,
        Guid id,
        UpdateProductDto dto);

    Task<bool> DeleteProduct(Guid userId, Guid id);
}