using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task AddAsync(ProductCategory category);
        Task UpdateAsync(ProductCategory category);
    }
}
