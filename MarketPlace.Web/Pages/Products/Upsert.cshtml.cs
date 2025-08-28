using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MarketPlace.Web.Pages.Products
{
    [Authorize]
    public class UpsertModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _categoryService;

        [BindProperty]
        public ProductDto Product { get; set; } = new ProductDto(Guid.Empty, string.Empty, 0m, Guid.Empty, string.Empty);

        public SelectList Categories { get; set; } = new SelectList(Enumerable.Empty<ProductCategoryDto>(), "Id", "Name");

        public UpsertModel(IProductService productService, IProductCategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryData = categories.Data ?? new List<ProductCategoryDto>();
            Categories = new SelectList(categoryData, "Id", "Name");


            if (id.HasValue)
            {
                var result = await _productService.GetProductByIdAsync(id.Value);
                if (!result.Success || result.Data is null)
                {
                    return NotFound();
                }
                Product = result.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryData = categories.Data ?? new List<ProductCategoryDto>();
            Categories = new SelectList(categoryData, "Id", "Name");


            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Product.Id == Guid.Empty) // CREATE
            {
                var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

                var create = new CreateProductRequest
                {
                    Name = Product.Name,
                    Price = Product.Price,
                    CategoryId = Product.CategoryId,
                    SellerId = sellerId,
                    Description = Product.Description
                };

                var result = await _productService.CreateProductAsync(create);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create product.");
                    return Page();
                }
            }
            else // UPDATE
            {
                var update = new UpdateProductRequest
                {
                    Id = Product.Id,
                    Name = Product.Name,
                    Price = Product.Price,
                    CategoryId = Product.CategoryId,
                    Description = Product.Description
                };

                var result = await _productService.UpdateProductAsync(update);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update product.");
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}
