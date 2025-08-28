using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> CreateProductAsync(CreateProductRequest request);
        Task<Result<ProductDto>> UpdateProductAsync(UpdateProductRequest request);
        Task<Result<ProductDto>> GetProductByIdAsync(Guid id);
        Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<Result<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId);
        Task<Result<bool>> ArchiveProductAsync(Guid id);
        Task<Result<IEnumerable<ProductDto>>> GetAllProductBySellerId(string id);
    }
}
