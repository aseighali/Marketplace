using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Interfaces
{
    public interface IProductCategoryService
    {
        Task<Result<IEnumerable<ProductCategoryDto>>> GetAllCategoriesAsync();
        Task<Result<ProductCategoryDto>> GetCategoryByIdAsync(Guid id);
        Task<Result<ProductCategoryDto>> CreateCategoryAsync(CreateProductCategoryRequest request);
        Task<Result<ProductCategoryDto>> UpdateCategoryAsync(UpdateProductCategoryRequest request);
        Task<Result<bool>> ArchiveCategoryAsync(Guid id);
    }
}
