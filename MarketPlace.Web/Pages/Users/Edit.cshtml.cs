using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Users
{
    [Authorize(Roles = UserRoles.Admin)]
    public class EditModel : PageModel
    {
        private readonly IAuthService _authService;

        public EditModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public EditUserRequest Input { get; set; } = new();

        public List<string> AllRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var result = await _authService.GetUserByIdAsync(id);
            if (!result.IsSuccess) 
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return NotFound();
            }

            var user = result.Value;

            Input = new EditUserRequest
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Role = user.Role
            };

            AllRoles = UserRoles.GetAllRoles().ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                AllRoles = UserRoles.GetAllRoles().ToList();
                return Page();
            }

            // Update display name
            var updateResult = await _authService.UpdateUserAsync(Input.Id, Input.DisplayName);
            if (!updateResult.IsSuccess)
            {
                TempData["ErrorMessage"] = updateResult.ErrorMessage;
                AllRoles = UserRoles.GetAllRoles().ToList();
                return Page();
            }

            // Update role
            var roleResult = await _authService.UpdateUserRoleAsync(Input.Id, Input.Role);
            if (!roleResult.IsSuccess)
            {
                TempData["ErrorMessage"] = roleResult.ErrorMessage;
                AllRoles = UserRoles.GetAllRoles().ToList();
                return Page();
            }

            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToPage("/Users/Index");
        }
    }
}
