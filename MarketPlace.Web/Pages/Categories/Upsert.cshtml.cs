using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public class UpsertModel : PageModel
    {
        private readonly IProductCategoryService _categoryService;

        public UpsertModel(IProductCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public ProductCategoryDto Category { get; set; } = new ProductCategoryDto(Guid.Empty, string.Empty);

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id.HasValue)
            {
                var result = await _categoryService.GetCategoryByIdAsync(id.Value);
                if (!result.Success || result.Data is null)
                {
                    return NotFound();
                }
                Category = result.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Category.Id == Guid.Empty) // CREATE
            {
                var create = new CreateProductCategoryRequest
                {
                    Name = Category.Name,
                };

                var result = await _categoryService.CreateCategoryAsync(create);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create category.");
                    return Page();
                }
            }
            else // UPDATE
            {
                var update = new UpdateProductCategoryRequest
                {
                    Id = Category.Id,
                    Name = Category.Name,
                };

                var result = await _categoryService.UpdateCategoryAsync(update);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update category.");
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}
