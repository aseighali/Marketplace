using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Repository;

namespace MarketPlace.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return Result<ProductDto>.Fail("Product not found.");

            return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
        }

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductRequest request)
        {
            try
            {
                var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    return Result<ProductDto>.Fail("Category not found.");
                }

                Product product = new Product(request.Name, request.Price, request.CategoryId, request.SellerId, request.Description);
                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();
                
                return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Fail(ex.Message);
            }
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(UpdateProductRequest request)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
                if (product == null)
                {
                    return Result<ProductDto>.Fail("Product not found.");
                }

                var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    return Result<ProductDto>.Fail("Category not found.");
                }

                product.Update(request.Name, request.Price, request.CategoryId, request.Description);
                await _unitOfWork.ProductRepository.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
            }
            catch (ArgumentException ex)
            {
                return Result<ProductDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Fail(ex.Message);
            }
        }

        public async Task<Result<bool>> ArchiveProductAsync(Guid productId)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    return Result<bool>.Fail("Product not found.");
                }

                product.Archive();
                await _unitOfWork.ProductRepository.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return Result<IEnumerable<ProductDto>>.Ok(dtos);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _unitOfWork.ProductRepository.GetByCategoryAsync(categoryId);
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return Result<IEnumerable<ProductDto>>.Ok(dtos);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductBySellerId(string id)
        {
            var products = await _unitOfWork.ProductRepository.GetBySellerIdAsync(id);
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return Result<IEnumerable<ProductDto>>.Ok(dtos);
        }
    }
}
