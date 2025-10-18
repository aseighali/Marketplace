using MarketPlace.Application.Interfaces;
using MarketPlace.Application.Common;
using MarketPlace.Infrastructure.Data;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly MarketPlaceAppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(MarketPlaceAppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<Result<ApplicationUser>> RegisterAsync(string email, string password, string? displayName = null, string? firstName = null, string? lastName = null)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                {
                    return Result<ApplicationUser>.FailureResult("User with this email already exists.");
                }

                // Validate password
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                {
                    return Result<ApplicationUser>.FailureResult("Password must be at least 6 characters long.");
                }

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                // Create new user
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email.ToLowerInvariant(),
                    PasswordHash = passwordHash,
                    DisplayName = displayName ?? email.Split('@')[0],
                    FirstName = firstName,
                    LastName = lastName,
                    Role = UserRoles.User, // Default role for new users
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Result<ApplicationUser>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.FailureResult($"Registration failed: {ex.Message}");
            }
        }

        public async Task<Result<ApplicationUser>> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant() && u.IsActive);
                if (user == null)
                {
                    return Result<ApplicationUser>.FailureResult("Invalid email or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return Result<ApplicationUser>.FailureResult("Invalid email or password.");
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Result<ApplicationUser>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.FailureResult($"Login failed: {ex.Message}");
            }
        }

        public async Task<Result<ApplicationUser>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
                if (user == null)
                {
                    return Result<ApplicationUser>.FailureResult("User not found.");
                }

                return Result<ApplicationUser>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.FailureResult($"Failed to get user: {ex.Message}");
            }
        }

        public async Task<Result<ApplicationUser>> UpdateUserAsync(string userId, string? displayName = null, string? firstName = null, string? lastName = null)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
                if (user == null)
                {
                    return Result<ApplicationUser>.FailureResult("User not found.");
                }

                if (!string.IsNullOrWhiteSpace(displayName))
                    user.DisplayName = displayName;
                if (!string.IsNullOrWhiteSpace(firstName))
                    user.FirstName = firstName;
                if (!string.IsNullOrWhiteSpace(lastName))
                    user.LastName = lastName;

                await _context.SaveChangesAsync();
                return Result<ApplicationUser>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.FailureResult($"Failed to update user: {ex.Message}");
            }
        }

        public async Task<Result<ApplicationUser>> UpdateUserRoleAsync(string userId, string newRole)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
                if (user == null)
                {
                    return Result<ApplicationUser>.FailureResult("User not found.");
                }

                // Validation happens in the ApplicationUser.Role setter
                user.Role = newRole;
                
                await _context.SaveChangesAsync();
                return Result<ApplicationUser>.SuccessResult(user);
            }
            catch (ArgumentException ex)
            {
                // Role validation failed
                return Result<ApplicationUser>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.FailureResult($"Failed to update user role: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
                if (user == null)
                {
                    return Result<bool>.FailureResult("User not found.");
                }

                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                {
                    return Result<bool>.FailureResult("Current password is incorrect.");
                }

                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return Result<bool>.FailureResult("New password must be at least 6 characters long.");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult($"Failed to change password: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return Result<bool>.FailureResult("User not found.");
                }

                user.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.FailureResult($"Failed to delete user: {ex.Message}");
            }
        }

        public async Task<Result<List<ApplicationUser>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.Where(u => u.IsActive).ToListAsync();
                return Result<List<ApplicationUser>>.SuccessResult(users);
            }
            catch (Exception ex)
            {
                return Result<List<ApplicationUser>>.FailureResult($"Failed to get users: {ex.Message}");
            }
        }

        public Task<StandarResult> CreateRefreshTokenForUser(Guid refreshToken, DateTime addDays, int userId)
        {
            //add this to database
            throw new NotImplementedException();


            //return success
        }

        public Task<Result<ApplicationUser>> GetUserByRefreshToken(string refreshtoken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRefresghTokenValid(string refreshtoken)
        {
            throw new NotImplementedException();
        }


        public class RefershToken
        {
            public int UserId { get; set; }
            public Guid Token { get; set; }
            public DateTime ExpireTime { get; set; }
            public DateTime CreationTime { get; set; }

            public int UsedCount { get; set; }
        }
    }
}
