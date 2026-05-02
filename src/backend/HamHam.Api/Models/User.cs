using System;

namespace HamHam.Api.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsActive { get; set; } = true;
        public string? Avatar { get; set; }
        public DateTimeOffset? LastSyncTime { get; set; }

        // Navigation Properties
        public virtual UserPreferences Preferences { get; set; } = null!;
    }
}
