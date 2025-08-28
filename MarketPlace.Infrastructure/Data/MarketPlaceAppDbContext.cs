using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using MarketPlace.Infrastructure.Entities;
using MarketPlace.Domain.Entities;


namespace MarketPlace.Infrastructure.Data
{
    public class MarketPlaceAppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public MarketPlaceAppDbContext( DbContextOptions<MarketPlaceAppDbContext> options ) : base( options ) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasIndex(z => z.CategoryId);
            builder.Entity<Product>().Property( z => z.Price )
                .HasColumnType("decimal(18,2)"); //for money 18 digits, 2 decimals

        }
    }
}
