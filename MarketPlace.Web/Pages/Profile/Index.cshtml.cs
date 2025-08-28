using MarketPlace.Application.Common;
using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Web.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;

        public UserInfoModel UserInfo { get; set; } = new();
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();

        public IndexModel(UserManager<ApplicationUser> userManager, IProductService productService)
        {
            _userManager = userManager;
            _productService = productService;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return;

            UserInfo = new UserInfoModel
            {
                Email = user.Email!,
                DisplayName = user.DisplayName
            };

            var result = await _productService.GetAllProductBySellerId(user.Id);
            if (result.Success)
            {
                Products = result.Data;
            }
            else
            {
                Products = new List<ProductDto>();
                TempData["ErrorMessage"] = result.Message ?? "Failed to load products.";
            }

        }

        public class UserInfoModel
        {
            public string Email { get; set; } = string.Empty;
            public string? DisplayName { get; set; }
        }
    }
}
