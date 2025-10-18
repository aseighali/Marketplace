using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
