using System;

namespace HamHam.Api.Models
{
    public enum WallpaperSource
    {
        Custom,
        Unsplash,
        Pexels,
        Mixed
    }

    public class UserPreferences : BaseEntity
    {
        public Guid Users_Id { get; set; }
        public WallpaperSource WallpaperSource { get; set; } = WallpaperSource.Mixed;
        public string[] WallpaperKeywords { get; set; } = Array.Empty<string>();
        public int RotationInterval { get; set; } = 24; // hours
        public bool IsWallpaperLocked { get; set; } = false;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}
