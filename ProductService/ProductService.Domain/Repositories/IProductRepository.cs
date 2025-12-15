using ProductService.Domain.Entities;

namespace ProductService.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);

        Task<List<Product>> GetAllAsync();

        Task AddAsync(Product product);

        void Update(Product product);

        void Remove(Product product);

        Task SaveChangesAsync();
    }
}
