using MarketPlace.Domain.Entities;

namespace MarketPlace.Infrastructure.Repository
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task AddAsync(ProductCategory category);
        Task UpdateAsync(ProductCategory category);
    }
}
