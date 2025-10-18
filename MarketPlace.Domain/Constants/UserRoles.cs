namespace MarketPlace.Domain.Constants
{
    /// <summary>
    /// Defines the available user roles in the system.
    /// Using constants prevents typos and ensures consistency.
    /// </summary>
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";



        /// <summary>
        /// Returns all valid roles in the system
        /// </summary>
        public static string[] GetAllRoles() => new[] { Admin, User };

        /// <summary>
        /// Validates if a role string is valid
        /// </summary>
        public static bool IsValidRole(string role)
        {
            return role == Admin || role == User;
        }
    }
}

