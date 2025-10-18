using MarketPlace.Application.DTOs;
using MarketPlace.Application.Interfaces;
using MarketPlace.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlace.Web.Pages.Users
{
    [Authorize(Roles = UserRoles.Admin)]
    public class UsersModel : PageModel
    {
        private readonly IAuthService _authService;

        public List<UserDto> Users { get; set; } = new();

        public UsersModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnGetAsync()
        {
            var result = await _authService.GetAllUsersAsync();
            if (result.IsSuccess)
            {
                Users = result.Value.Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsActive = user.IsActive
                }).ToList();
            }
        }
    }
}
