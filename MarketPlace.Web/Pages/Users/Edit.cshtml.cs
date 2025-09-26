using MarketPlace.Application.DTOs;
using MarketPlace.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public EditUserRequest Input { get; set; } = new();

        public List<string> AllRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            Input = new EditUserRequest
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Role = roles.FirstOrDefault() ?? "User"
            };

            AllRoles = _roleManager.Roles.Select(r => r.Name!).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(Input.Id);
            if (user == null) return NotFound();

            // update display name
            user.DisplayName = Input.DisplayName;
            await _userManager.UpdateAsync(user);

            // update role
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            await _userManager.AddToRoleAsync(user, Input.Role);

            return RedirectToPage("/Users/Index");
        }

    }
}
