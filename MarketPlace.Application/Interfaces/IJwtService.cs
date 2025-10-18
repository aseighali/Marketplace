using MarketPlace.Domain.Entities;

namespace MarketPlace.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
