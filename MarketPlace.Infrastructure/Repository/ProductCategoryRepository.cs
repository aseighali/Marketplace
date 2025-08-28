using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly MarketPlaceAppDbContext _context;
        public ProductCategoryRepository(MarketPlaceAppDbContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(ProductCategory category)
        {
            await _context.ProductCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        /*public async Task ArchiveAsync(Guid id)
        {
            var productCategory = await GetByIdAsync(id);
            if (productCategory != null) 
            {
                productCategory.IsArchived = true;
                await _context.SaveChangesAsync();
            }
        }*/

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory?> GetByIdAsync(Guid id)
        {
            return await _context.ProductCategories.FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task UpdateAsync(ProductCategory category)
        {
            await UpdateAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
