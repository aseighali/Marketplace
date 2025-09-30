using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly CartService _cartService;
        public IndexModel(IProductService productService, CartService cartService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
        public Application.DTOs.CartDTO Cart { get; set; } = new();

        public async Task OnGetAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            if (result.Success)
            {
                Products = result.Data;
                Cart = _cartService.GetCart();
            }
            else
            {
                Products = new List<ProductDto>();
                TempData["ErrorMessage"] = result.Message ?? "Failed to load products.";
            }
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid Id)
        {
            var productResult = await _productService.GetProductByIdAsync(Id);

            if (productResult.Success && productResult.Data != null)
            {
                var product = productResult.Data;
                _cartService.AddToCart(new CartItem { Name = product.Name, Price = product.Price, ProductId = product.Id});
                TempData["SuccessMessage"] = $"{productResult.Data.Name} added to cart.";
            }
            else
            {
                TempData["ErrorMessage"] = "Product could not be added to cart.";
            }

            return RedirectToPage("/Products/Index");
        }


    }
}
