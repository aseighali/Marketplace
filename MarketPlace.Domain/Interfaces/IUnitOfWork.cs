namespace MarketPlace.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
