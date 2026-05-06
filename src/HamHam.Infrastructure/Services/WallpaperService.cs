using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using HamHam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace HamHam.Infrastructure.Services
{
    public class WallpaperService : IWallpaperService
    {
        private readonly HamHamDbContext _context;
        private readonly IConnectionMultiplexer _redis;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUnsplashService _unsplashService;
        private readonly string _storagePath;

        public WallpaperService(HamHamDbContext context, IConnectionMultiplexer redis, IHttpClientFactory httpClientFactory, IConfiguration configuration, IUnsplashService unsplashService)
        {
            _context = context;
            _redis = redis;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _unsplashService = unsplashService;
            
            _storagePath = Path.Combine(AppContext.BaseDirectory, "uploads", "wallpapers");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> GetCurrentWallpaperAsync(Guid userId)
        {
            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UsersId == userId);
            if (pref == null) return "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1920"; // Default
        
            if (pref.WallpaperType == WallpaperType.Unsplash)
            {
                var elapsed = DateTime.UtcNow - pref.WallpaperLastUpdated;
                if (elapsed.TotalHours >= pref.RotationInterval || string.IsNullOrEmpty(pref.WallpaperValue))
                {
                    return await RefreshWallpaperAsync(userId);
                }
                return pref.WallpaperValue;
            }
        
            return pref.WallpaperType switch
            {
                WallpaperType.Color => pref.WallpaperValue,
                WallpaperType.Image => pref.WallpaperValue,
                _ => "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1920"
            };
        }
        
        public async Task<string> RefreshWallpaperAsync(Guid userId)
        {
            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UsersId == userId);
            if (pref == null) return "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1920";
        
            if (pref.WallpaperType != WallpaperType.Unsplash)
            {
                return pref.WallpaperValue;
            }

            var source = pref.WallpaperSource;
            var keywords = pref.WallpaperKeywords;
            
            string cacheKey = $"wallpaper:pool:{(source == WallpaperSource.Unsplash ? "unsplash" : "pexels")}:{string.Join(",", keywords)}";
            var db = _redis.GetDatabase();
            var cachedPool = await db.StringGetAsync(cacheKey);
        
            List<string> urls;
            if (!cachedPool.IsNullOrEmpty)
            {
                urls = System.Text.Json.JsonSerializer.Deserialize<List<string>>(cachedPool!);
            }
            else
            {
                urls = await _unsplashService.GetRandomWallpapersAsync(keywords);
                await db.StringSetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(urls), TimeSpan.FromHours(24));
            }
        
            var newUrl = urls[Random.Shared.Next(urls.Count)];
            pref.WallpaperLastUpdated = DateTime.UtcNow;
            pref.WallpaperValue = newUrl;
            pref.WallpaperType = WallpaperType.Unsplash;
            pref.WallpaperSource = WallpaperSource.Unsplash;
            await _context.SaveChangesAsync();
        
            return newUrl;
        }

        public async Task<string> UploadCustomWallpaperAsync(Guid userId, byte[] fileContent, string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var fileNameUnique = $"{userId}_{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(_storagePath, fileNameUnique);
        
            await File.WriteAllBytesAsync(fullPath, fileContent);
        
            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UsersId == userId);
            if (pref == null)
            {
                pref = new UserPreference { Id = Guid.NewGuid(), UsersId = userId };
                _context.UserPreferences.Add(pref);
            }
        
            pref.WallpaperSource = WallpaperSource.Custom;
            pref.WallpaperType = WallpaperType.Image;
            pref.WallpaperValue = $"/uploads/wallpapers/{fileNameUnique}";
            await _context.SaveChangesAsync();
        
            return pref.WallpaperValue;
        }

        public async Task UpdateWallpaperPreferencesAsync(Guid userId, UpdateWallpaperPreferencesRequest request)
        {
            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UsersId == userId);
            if (pref == null)
            {
                pref = new UserPreference { Id = Guid.NewGuid(), UsersId = userId };
                _context.UserPreferences.Add(pref);
            }
        
            pref.WallpaperType = request.Type;
            pref.WallpaperValue = request.Value;
            pref.OverlayOpacity = request.OverlayOpacity;

            if (request.Type == WallpaperType.Unsplash)
            {
                pref.WallpaperSource = request.Source;
                pref.WallpaperKeywords = request.Keywords;
                pref.RotationInterval = request.RotationInterval;
            }
            else
            {
                pref.WallpaperSource = WallpaperSource.Custom;
                pref.WallpaperKeywords = Array.Empty<string>();
                pref.RotationInterval = 24;
            }
    
            await _context.SaveChangesAsync();
        }
        
        public async Task<UserPreference> GetPreferencesAsync(Guid userId)
        {
            var pref = await _context.UserPreferences.FirstOrDefaultAsync(p => p.UsersId == userId);
            if (pref == null)
            {
                return new UserPreference 
                { 
                    WallpaperType = WallpaperType.Image, 
                    WallpaperValue = "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1920",
                    WallpaperSource = WallpaperSource.Custom,
                    RotationInterval = 24
                };
            }
            return pref;
        }
    }
}