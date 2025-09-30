using MarketPlace.Application.Interfaces;
using MarketPlace.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();

            return services;
        }
    }
}
