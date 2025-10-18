using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarketPlace.Web.Services;

namespace MarketPlace.Web.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly WebAuthService _authService;

        public LogoutModel(WebAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _authService.Logout();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _authService.Logout();
            return RedirectToPage("/Index");
        }
    }
}
