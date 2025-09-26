using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Application.DTOs
{
    public class EditUserRequest
    {
        public string Id { get; set; } = string.Empty;
        
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = "User";
    }
}
