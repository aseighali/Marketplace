using Microsoft.AspNetCore.Mvc;
using MarketPlace.Application.Interfaces;
using MarketPlace.Application.DTOs;
using MarketPlace.Web.Extensions;
using MarketPlace.Domain.Entities;

namespace MarketPlace.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid request data."
                });
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Password and confirm password do not match."
                });
            }

            var result = await _authService.RegisterAsync(
                request.Email,
                request.Password,
                request.DisplayName,
                request.FirstName,
                request.LastName
            );

            if (!result.IsSuccess)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            var token = _jwtService.GenerateToken(result.Value);
            var userInfo = MapToUserInfoDto(result.Value);

            return Ok(new AuthResponse
            {
                Success = true,
                Token = token,
                User = userInfo,
                Message = "Registration successful."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid request data."
                });
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (!result.IsSuccess)
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            var token = _jwtService.GenerateToken(result.Value);
            var userInfo = MapToUserInfoDto(result.Value);

            // Set HTTP-only cookie for browser-based authentication
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            Response.Cookies.Append("auth_token", token, cookieOptions);

            return Ok(new AuthResponse
            {
                Success = true,
                Token = token,
                User = userInfo,
                Message = "Login successful."
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Logout successful."
            });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _authService.GetUserByIdAsync(userId);
            if (!result.IsSuccess)
            {
                return NotFound(new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            var userInfo = MapToUserInfoDto(result.Value);
            return Ok(new AuthResponse
            {
                Success = true,
                User = userInfo
            });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _authService.UpdateUserAsync(
                userId,
                request.DisplayName,
                request.FirstName,
                request.LastName
            );

            if (!result.IsSuccess)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            var userInfo = MapToUserInfoDto(result.Value);
            return Ok(new AuthResponse
            {
                Success = true,
                User = userInfo,
                Message = "Profile updated successfully."
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Invalid request data."
                });
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "New password and confirm password do not match."
                });
            }

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "User not authenticated."
                });
            }

            var result = await _authService.ChangePasswordAsync(
                userId,
                request.CurrentPassword,
                request.NewPassword
            );

            if (!result.IsSuccess)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = result.ErrorMessage
                });
            }

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Password changed successfully."
            });
        }

        private static UserInfoDto MapToUserInfoDto(ApplicationUser user)
        {
            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}
