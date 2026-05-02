using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface IWallpaperService
    {
    Task<string> GetCurrentWallpaperAsync(Guid userId);
    Task<string> RefreshWallpaperAsync(Guid userId);
    Task<string> UploadCustomWallpaperAsync(Guid userId, byte[] fileContent, string fileName);
    Task UpdateWallpaperPreferencesAsync(Guid userId, UpdateWallpaperPreferencesRequest request);
    Task<UserPreference> GetPreferencesAsync(Guid userId);
    }

    public record UpdateWallpaperPreferencesRequest(WallpaperType Type, string Value, WallpaperSource Source, string[] Keywords, int RotationInterval, double OverlayOpacity);
}
