using MarketPlace.Infrastructure.Repository;
using MarketPlace.Infrastructure.Data;

namespace MarketPlace.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MarketPlaceAppDbContext _context;
        private IProductRepository? _productRepository;
        private IProductCategoryRepository? _productCategoryRepository;

        public UnitOfWork(MarketPlaceAppDbContext context)
        {
            _context = context;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                _productRepository ??= new ProductRepository(_context);
                return _productRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                _productCategoryRepository ??= new ProductCategoryRepository(_context);
                return _productCategoryRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
