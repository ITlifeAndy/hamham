using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/wallpaper")]
    [Authorize]
    public class WallpaperController : ControllerBase
    {
        public class WallpaperUploadRequest
        {
            public IFormFile File { get; set; }
        }

        private readonly IWallpaperService _wallpaperService;

        public WallpaperController(IWallpaperService wallpaperService)
        {
            _wallpaperService = wallpaperService;
        }

        private Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var url = await _wallpaperService.GetCurrentWallpaperAsync(UserId);
            return Ok(new { Url = url });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var url = await _wallpaperService.RefreshWallpaperAsync(UserId);
            return Ok(new { Url = url });
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] WallpaperUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0) return BadRequest("File is required");

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);
            var content = ms.ToArray();

            var path = await _wallpaperService.UploadCustomWallpaperAsync(UserId, content, request.File.FileName);
            return Ok(new { Path = path });
        }

        [HttpPatch("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] UpdateWallpaperPreferencesRequest request)
        {
            await _wallpaperService.UpdateWallpaperPreferencesAsync(UserId, request);
            return Ok();
        }
    
        [HttpGet("preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var prefs = await _wallpaperService.GetPreferencesAsync(UserId);
            return Ok(prefs);
        }
    }
}
