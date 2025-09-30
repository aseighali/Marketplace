using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all categories
        public async Task<Result<IEnumerable<ProductCategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            var dtos = categories.Select(c => ProductCategoryDto.FromDomain(c));
            return Result<IEnumerable<ProductCategoryDto>>.Ok(dtos);
        }

        // Get category by Id
        public async Task<Result<ProductCategoryDto>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id);
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

            try
            {
                var category = new ProductCategory(request.Name);
                await _unitOfWork.ProductCategoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return Result<ProductCategoryDto>.Ok(ProductCategoryDto.FromDomain(category));
            }
            catch (Exception ex)
            {
                return Result<ProductCategoryDto>.Fail(ex.Message);
            }
        }

        // Update existing category
        public async Task<Result<ProductCategoryDto>> UpdateCategoryAsync(UpdateProductCategoryRequest request)
        {
            try
            {
                var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(request.Id);
                if (category == null)
                {
                    return Result<ProductCategoryDto>.Fail("Category not found.");
                }

                category.Update(request.Name);
                await _unitOfWork.ProductCategoryRepository.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return Result<ProductCategoryDto>.Ok(ProductCategoryDto.FromDomain(category));
            }
            catch (Exception ex)
            {
                return Result<ProductCategoryDto>.Fail(ex.Message);
            }
        }

        // Archive category instead of delete
        public async Task<Result<bool>> ArchiveCategoryAsync(Guid id)
        {
            try
            {
                var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return Result<bool>.Fail("Category not found.");
                }

                category.Archive();
                await _unitOfWork.ProductCategoryRepository.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Ok(true, "Category archived successfully.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }
    }
}
