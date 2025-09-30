namespace MarketPlace.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
