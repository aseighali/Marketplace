using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
    }
}
