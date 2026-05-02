using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HamHam.Application.Interfaces;

namespace HamHam.Infrastructure.Services
{
    public class UnsplashService : IUnsplashService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UnsplashService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<string>> GetRandomWallpapersAsync(string[] keywords)
        {
            var client = _httpClientFactory.CreateClient();
            var apiKey = _configuration["Unsplash:AccessKey"];
            
            if (string.IsNullOrEmpty(apiKey))
            {
                return GetFallbackWallpapers();
            }

            var query = keywords.Length > 0 ? keywords[0] : "nature";
            var url = $"https://api.unsplash.com/photos/random?query={query}&count=10&client_id={apiKey}";

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var photos = await response.Content.ReadFromJsonAsync<List<UnsplashPhoto>>();
                    return photos?.Select(p => p.Urls.Regular).ToList() ?? GetFallbackWallpapers();
                }
                
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    // Log rate limit exceeded
                    return GetFallbackWallpapers();
                }
            }
            catch (Exception)
            {
                // Log exception
            }

            return GetFallbackWallpapers();
        }

        private List<string> GetFallbackWallpapers()
        {
            return new List<string>
            {
                "https://images.unsplash.com/photo-1501854140801-50d01698950b?q=80&w=1920",
                "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?q=80&w=1920",
                "https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?q=80&w=1920"
            };
        }

        private class UnsplashPhoto
        {
            public UnsplashUrls Urls { get; set; } = new();
        }

        private class UnsplashUrls
        {
            public string Regular { get; set; } = string.Empty;
        }
    }
}
