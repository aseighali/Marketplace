using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarketPlace.Application.DTOs;
using MarketPlace.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly WebAuthService _authService;

        public LoginModel(WebAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new();

        [BindProperty]
        public bool RememberMe { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            // Store return URL for after login
            ViewData["ReturnUrl"] = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _authService.LoginAsync(LoginRequest.Email, LoginRequest.Password, RememberMe);

            if (result.Success)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Login failed.");
            return Page();
        }
    }
}
