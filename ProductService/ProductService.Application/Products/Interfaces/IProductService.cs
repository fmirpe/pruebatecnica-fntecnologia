using ProductService.Application.Products.DTOs;

namespace ProductService.Application.Products.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductRequest request);
        Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request);
        Task DeleteAsync(Guid id);
        Task<ProductDto> GetByIdAsync(Guid id);
        Task<List<ProductDto>> GetAllAsync();
    }
}
