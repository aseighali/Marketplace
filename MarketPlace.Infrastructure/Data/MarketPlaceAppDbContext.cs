using Microsoft.EntityFrameworkCore;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Infrastructure.Data
{
    public class MarketPlaceAppDbContext : DbContext
    {
        public MarketPlaceAppDbContext(DbContextOptions<MarketPlaceAppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("User");
            });

            // Configure Product entity
            builder.Entity<Product>().HasIndex(z => z.CategoryId);
            builder.Entity<Product>().Property(z => z.Price)
                .HasColumnType("decimal(18,2)"); //for money 18 digits, 2 decimals
        }
    }
}
