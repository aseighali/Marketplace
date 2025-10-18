using MarketPlace.Application.Common;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<ApplicationUser>> RegisterAsync(string email, string password, string? displayName = null, string? firstName = null, string? lastName = null);
        Task<Result<ApplicationUser>> LoginAsync(string email, string password);
        Task<Result<ApplicationUser>> GetUserByIdAsync(string userId);
        Task<Result<ApplicationUser>> UpdateUserAsync(string userId, string? displayName = null, string? firstName = null, string? lastName = null);
        Task<Result<ApplicationUser>> UpdateUserRoleAsync(string userId, string newRole);
        Task<Result<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<Result<bool>> DeleteUserAsync(string userId);
        Task<Result<List<ApplicationUser>>> GetAllUsersAsync();
        Task<StandarResult> CreateRefreshTokenForUser(Guid refreshToken, DateTime addDays, int userId);
        Task<Result<ApplicationUser>> GetUserByRefreshToken(string refreshtoken);
        Task<bool> IsRefresghTokenValid(string refreshtoken);
    }


    public class StandarResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public bool HasError { get; set; }

        public List<string> Erros { get; set; }
    }
}
