using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductCategoryService(IProductCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Get all categories
        public async Task<Result<IEnumerable<ProductCategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtos = categories.Select(c => ProductCategoryDto.FromDomain(c));
            return Result<IEnumerable<ProductCategoryDto>>.Ok(dtos);
        }

        // Get category by Id
        public async Task<Result<ProductCategoryDto>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return Result<ProductCategoryDto>.Fail("Category not found.");

            return Result<ProductCategoryDto>.Ok(ProductCategoryDto.FromDomain(category));
        }

        // Create a new category
        public async Task<Result<ProductCategoryDto>> CreateCategoryAsync(CreateProductCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<ProductCategoryDto>.Fail("Category name is required.");

            if (request.Name.Length > 100)
                return Result<ProductCategoryDto>.Fail("Category name cannot exceed 100 characters.");

            var category = new ProductCategory(request.Name);

            await _categoryRepository.AddAsync(category);

            return Result<ProductCategoryDto>.Ok(ProductCategoryDto.FromDomain(category));
        }

        // Update existing category
        public async Task<Result<ProductCategoryDto>> UpdateCategoryAsync(UpdateProductCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
                return Result<ProductCategoryDto>.Fail("Category not found.");

            category.Update(request.Name);

            await _categoryRepository.UpdateAsync(category);

            return Result<ProductCategoryDto>.Ok(ProductCategoryDto.FromDomain(category));
        }

        // Archive category instead of delete
        public async Task<Result<bool>> ArchiveCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return Result<bool>.Fail("Category not found.");

            category.Archive();
            await _categoryRepository.UpdateAsync(category);

            return Result<bool>.Ok(true, "Category archived successfully.");
        }
    }
}
