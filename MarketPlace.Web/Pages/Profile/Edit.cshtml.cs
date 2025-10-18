using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Profile
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public ProfileEditRequest Input { get; set; } = new();

        public EditModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return NotFound();

            var result = await _authService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return NotFound();
            }

            Input = new ProfileEditRequest
            {
                DisplayName = result.Value.DisplayName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return NotFound();

            var result = await _authService.UpdateUserAsync(userId, Input.DisplayName);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return Page();
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToPage("Index");
        }
    }
}
