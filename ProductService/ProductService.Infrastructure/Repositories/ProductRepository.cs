
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.AsNoTracking().ToListAsync();
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Remove(Product product)
        {
            _context.Products.Remove(product);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
