using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IProductService _productService;

        public UserInfoDto UserInfo { get; set; } = new();
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();

        public IndexModel(IAuthService authService, IProductService productService)
        {
            _authService = authService;
            _productService = productService;
        }

        public async Task OnGetAsync()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return;

            var userResult = await _authService.GetUserByIdAsync(userId);
            if (!userResult.IsSuccess) return;

            var user = userResult.Value;
            UserInfo = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };

            var result = await _productService.GetAllProductBySellerId(userId);
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
    }
}
