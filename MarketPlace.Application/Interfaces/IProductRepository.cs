using MarketPlace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);

        Task<IEnumerable<Product>> GetBySellerIdAsync(string id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        /*Task ArchiveAsync(Guid id);*/
    }
}
