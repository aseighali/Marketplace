using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Application.Services;
using MarketPlace.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IProductCategoryService _categoryService;

        public IndexModel(IProductCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
        public async Task OnGetAsync()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (result.Success)
            {
                Categories = result.Data;
            }
            else
            {
                Categories = new List<ProductCategoryDto>();
                TempData["ErrorMessage"] = result.Message ?? "Failed to load product categories.";
            }
        }

    }
}
