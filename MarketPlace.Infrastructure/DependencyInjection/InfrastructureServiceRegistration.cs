using MarketPlace.Application.Interfaces;
using MarketPlace.Infrastructure.Data;
using MarketPlace.Infrastructure.Entities;
using MarketPlace.Infrastructure.Repository;
using MarketPlace.Infrastructure.Repository.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<MarketPlaceAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity Services
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<MarketPlaceAppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            // Repository Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            return services;
        }

        public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MarketPlaceAppDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}
