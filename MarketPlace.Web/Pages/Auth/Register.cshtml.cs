using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarketPlace.Application.DTOs;
using MarketPlace.Web.Services;

namespace MarketPlace.Web.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly WebAuthService _authService;

        public RegisterModel(WebAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (RegisterRequest.Password != RegisterRequest.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(RegisterRequest.ConfirmPassword), "Password and confirm password do not match.");
                return Page();
            }

            var result = await _authService.RegisterAsync(
                RegisterRequest.Email,
                RegisterRequest.Password,
                RegisterRequest.DisplayName,
                RegisterRequest.FirstName,
                RegisterRequest.LastName
            );

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToPage("/Auth/Login");
            }

            ModelState.AddModelError(string.Empty, result.Message ?? "Registration failed.");
            return Page();
        }
    }
}
