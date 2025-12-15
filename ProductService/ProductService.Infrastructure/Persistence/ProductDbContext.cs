

using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var product = modelBuilder.Entity<Product>();

            product.ToTable("Products");

            product.HasKey(p => p.Id);

            product.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            product.Property(p => p.Description)
                .HasMaxLength(1000);

            product.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            product.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(100);

            product.Property(p => p.IsActive)
                .IsRequired();

            product.Property(p => p.CreatedAt)
                .IsRequired();

            product.Property(p => p.UpdatedAt)
                .IsRequired(false);
        }
    }
}
