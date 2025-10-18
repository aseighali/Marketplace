using MarketPlace.Application.Interfaces;
using MarketPlace.Application.DTOs;
using MarketPlace.Domain.Entities;
using Azure;

namespace MarketPlace.Web.Services
{
    public class WebAuthService
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebAuthService(IAuthService authService, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponse> LoginAsync(string email, string password, bool rememberMe = false)
        {
            var result = await _authService.LoginAsync(email, password);
            
            if (!result.IsSuccess)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                };
            }

            var user = result.Value;
            var token = _jwtService.GenerateToken(user);
            
            // Store JWT token in HTTP-only cookie for web pages
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = httpContext.Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
                };
                httpContext.Response.Cookies.Append("auth_token", token, cookieOptions);
            }

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                },
                Message = "Login successful"
            };
        }

        public async Task<AuthResponse> RegisterAsync(string email, string password, string? displayName = null, string? firstName = null, string? lastName = null)
        {
            var result = await _authService.RegisterAsync(email, password, displayName, firstName, lastName);
            
            if (!result.IsSuccess)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                };
            }

            var user = result.Value;
            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                },
                Message = "Registration successful"
            };
        }

        public async Task Logout()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Remove JWT token cookie
                httpContext.Response.Cookies.Delete("auth_token");
            }
            await Task.CompletedTask; // Keep async signature
        }

        public async Task<AuthResponse> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var result = await _authService.ChangePasswordAsync(userId, currentPassword, newPassword);
            
            return new AuthResponse
            {
                Success = result.IsSuccess,
                Message = result.ErrorMessage
            };
        }

        public async Task<AuthResponse> UpdateProfileAsync(string userId, string? displayName = null, string? firstName = null, string? lastName = null)
        {
            var result = await _authService.UpdateUserAsync(userId, displayName, firstName, lastName);
            
            if (!result.IsSuccess)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                };
            }

            var user = result.Value;
            return new AuthResponse
            {
                Success = true,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt
                },
                Message = "Profile updated successfully"
            };
        }

        /// <summary>
        /// call it after succesfull login
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SetRefreshToken(int userId)
        {
            var refreshToken = Guid.NewGuid();
            var result = _authService.CreateRefreshTokenForUser(refreshToken, DateTime.Now.AddDays(1), userId);

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-refreshToken", refreshToken.ToString(), new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMonths(1),
                Secure = true,

                //TODO: enable the domain
                Domain = ".domain.com",
            });
        }
        public async Task<string> RenewRefreshToken()
        {
            string refreshtoken = _httpContextAccessor.HttpContext.Request.Headers["X-RefreshToken"];

            var user = _authService.GetUserByRefreshToken(refreshtoken);


            if (await _authService.IsRefresghTokenValid(refreshtoken))
            {
                //create and return new jwt
            }

            throw new NotImplementedException();
        }
    }
}
