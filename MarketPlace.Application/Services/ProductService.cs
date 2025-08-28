using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductCategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        public ProductService(IProductCategoryRepository productCategoryRepo, IProductRepository productRepository)
        {
            _categoryRepo = productCategoryRepo;
            _productRepo = productRepository;

        }
        public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return Result<ProductDto>.Fail("Product not found.");

            return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
        }

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductRequest request)
        {
            var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return Result<ProductDto>.Fail("Category not found.");
            }

            Product product;
            try
            {
                product = new Product(request.Name, request.Price, request.CategoryId, request.SellerId, request.Description);
                await _productRepo.AddAsync(product);

            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Fail(ex.Message);
            }
            return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(UpdateProductRequest request)
        {
            var product = await _productRepo.GetByIdAsync(request.Id);
            if (product == null)
                return Result<ProductDto>.Fail("Product not found.");


            var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<ProductDto>.Fail("Category not found.");

            try
            {
                product.Update(request.Name, request.Price, request.CategoryId, request.Description);
                await _productRepo.UpdateAsync(product);
            }
            catch (ArgumentException ex)
            {
                return Result<ProductDto>.Fail(ex.Message);
            }

            return Result<ProductDto>.Ok(ProductDto.FromDomain(product));
        }

        public async Task<Result<bool>> ArchiveProductAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return Result<bool>.Fail("Product not found.");

            product.Archive();
            await _productRepo.UpdateAsync(product);

            return Result<bool>.Ok(true);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return  Result<IEnumerable<ProductDto>>.Ok(dtos);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepo.GetByCategoryAsync(categoryId);
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return Result<IEnumerable<ProductDto>>.Ok(dtos);
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductBySellerId(string id)
        {
            var products = await _productRepo.GetBySellerIdAsync(id);
            var dtos = products.Select(z => ProductDto.FromDomain(z));
            return Result<IEnumerable<ProductDto>>.Ok(dtos);

        }
    }
}
