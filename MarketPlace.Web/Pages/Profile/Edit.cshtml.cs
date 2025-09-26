using MarketPlace.Application.DTOs;
using MarketPlace.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Profile
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public ProfileEditRequest Input { get; set; } = new();

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            Input = new ProfileEditRequest
            {
                DisplayName = user.DisplayName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.DisplayName = Input.DisplayName;
            await _userManager.UpdateAsync(user);

            return RedirectToPage("Index");
        }

    }
}
