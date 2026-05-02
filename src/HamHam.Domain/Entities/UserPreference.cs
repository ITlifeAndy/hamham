using System;

namespace HamHam.Domain.Entities
{
    public class UserPreference : BaseEntity
    {
        public Guid UsersId { get; set; }
        public WallpaperType WallpaperType { get; set; } = WallpaperType.Unsplash;
        public string WallpaperValue { get; set; } = string.Empty;
        public WallpaperSource WallpaperSource { get; set; } = WallpaperSource.Mixed;
        public string[] WallpaperKeywords { get; set; } = Array.Empty<string>();
        public int RotationInterval { get; set; } = 24; // hours
        public double OverlayOpacity { get; set; } = 0.4;
        public bool IsWallpaperLocked { get; set; } = false;
        public DateTime WallpaperLastUpdated { get; set; } = DateTime.UtcNow;
    }
}
