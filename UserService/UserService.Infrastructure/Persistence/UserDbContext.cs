
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var userEntity = modelBuilder.Entity<User>();

            userEntity.ToTable("Users");

            userEntity.HasKey(u => u.Id);

            userEntity.Property(u => u.Id)
                .IsRequired();

            userEntity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            userEntity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            userEntity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            userEntity.Property(u => u.Role)
                .IsRequired()
                .HasConversion<int>();

            userEntity.Property(u => u.CreatedAt)
                .IsRequired();

            userEntity.Property(u => u.UpdatedAt)
                .IsRequired(false);

            userEntity.HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
