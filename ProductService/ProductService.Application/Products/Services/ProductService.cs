using ProductService.Application.Products.DTOs;
using ProductService.Application.Products.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;

namespace ProductService.Application.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequest request)
        {
            if (request.Price < 0)
            {
                throw new InvalidOperationException("Price cannot be negative.");
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Price = request.Price,
                Category = request.Category.Trim(),
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            if (request.Price < 0)
            {
                throw new InvalidOperationException("Price cannot be negative.");
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            existing.Name = request.Name.Trim();
            existing.Description = request.Description?.Trim();
            existing.Price = request.Price;
            existing.Category = request.Category.Trim();
            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            _repository.Update(existing);
            await _repository.SaveChangesAsync();

            return MapToDto(existing);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _repository.Remove(existing);
            await _repository.SaveChangesAsync();
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            return MapToDto(product);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        private static ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                IsActive = product.IsActive
            };
        }
    }
}
