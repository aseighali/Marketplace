using System.ComponentModel.DataAnnotations;
using MarketPlace.Domain.Constants;

namespace MarketPlace.Domain.Entities
{
    public class ApplicationUser
    {
        private string _role = UserRoles.User;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public string? DisplayName { get; set; }
        
        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// User's role in the system. Defaults to User.
        /// Valid roles are defined in UserRoles constants.
        /// </summary>
        [Required]
        public string Role 
        { 
            get => _role;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Role cannot be empty");
                }
                
                if (!UserRoles.IsValidRole(value))
                {
                    throw new ArgumentException($"Invalid role: {value}. Valid roles are: {string.Join(", ", UserRoles.GetAllRoles())}");
                }
                
                _role = value;
            }
        }
    }
}
