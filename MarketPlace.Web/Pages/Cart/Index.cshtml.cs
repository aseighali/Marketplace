using MarketPlace.Application.DTOs;
using MarketPlace.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly CartService _cartService;

        public Application.DTOs.Cart Cart { get; set; } = new();

        public IndexModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public void OnGet()
        {
            Cart = _cartService.GetCart();
        }

        public IActionResult OnPostRemove(Guid id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            _cartService.ClearCart();
            return RedirectToPage();
        }
    }
}
