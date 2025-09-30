using MarketPlace.Domain.Entities;

namespace MarketPlace.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetBySellerIdAsync(string id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
    }
}
