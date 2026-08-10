using ng_ecommerce_api.DTOs.Products;

namespace ng_ecommerce_api.Services;

public interface IProductService
{
    IEnumerable<ProductDto> GetAllProducts();

    ProductDto? GetProductById(Guid id);
    ProductDto? GetProductByName(string name);
    ProductDto? SearchProductByName(string name);
    List<ProductDto> GetProductByCategory(string category);
    

    ProductDto CreateProduct(CreateProductDto dto);

    bool UpdateProduct(Guid id, UpdateProductDto dto);

    bool DeleteProduct(Guid id);
}