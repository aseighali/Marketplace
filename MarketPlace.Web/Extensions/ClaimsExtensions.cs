using System.Security.Claims;

namespace MarketPlace.Web.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static string? GetDisplayName(this ClaimsPrincipal user)
        {
            return user.FindFirst("DisplayName")?.Value;
        }
    }
}
