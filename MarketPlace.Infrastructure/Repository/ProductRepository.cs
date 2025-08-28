using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MarketPlaceAppDbContext _context;
        public ProductRepository(MarketPlaceAppDbContext context) 
        {
            _context = context;
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(z => z.Category).ToListAsync();//as no tracking???
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)//as no tracking???
        {
            return await _context.Products.Include(z => z.Category).Where(z => z.CategoryId == categoryId).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)//as no tracking???
        {
            return await _context.Products.Include(z => z.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetBySellerIdAsync(string id)
        {
            return await _context.Products.Include(z => z.Category).Where(z => z.SellerId == id).ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
