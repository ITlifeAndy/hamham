using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/shared")]
    public class SharedLibraryController : ControllerBase
    {
        private readonly ISharedLibraryService _sharedLibraryService;

        public SharedLibraryController(ISharedLibraryService sharedLibraryService)
        {
            _sharedLibraryService = sharedLibraryService;
        }

        private Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet("pools")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPools()
        {
            var pools = await _sharedLibraryService.GetPublicPoolsAsync();
            return Ok(pools);
        }

        [HttpGet("pools/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPool(Guid id)
        {
            var pool = await _sharedLibraryService.GetPoolByIdAsync(id);
            if (pool == null) return NotFound();
            return Ok(pool);
        }

        [HttpGet("pools/{id}/bookmarks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPoolBookmarks(Guid id)
        {
            var bookmarks = await _sharedLibraryService.GetPoolBookmarksAsync(id);
            return Ok(bookmarks);
        }

        [Authorize]
        [HttpPost("pools")]
        public async Task<IActionResult> CreatePool([FromBody] HamHam.Application.Interfaces.CreatePoolRequest request)
        {
            var pool = await _sharedLibraryService.CreatePoolAsync(UserId, request);
            return Ok(pool);
        }

        [Authorize]
        [HttpPatch("pools/{id}")]
        public async Task<IActionResult> UpdatePool(Guid id, [FromBody] HamHam.Application.Interfaces.UpdatePoolRequest request)
        {
            var success = await _sharedLibraryService.UpdatePoolAsync(UserId, id, request);
            return success ? Ok() : NotFound();
        }

        [Authorize]
        [HttpDelete("pools/{id}")]
        public async Task<IActionResult> DeletePool(Guid id)
        {
            var success = await _sharedLibraryService.DeletePoolAsync(UserId, id);
            return success ? Ok() : NotFound();
        }

        [Authorize]
        [HttpPost("pick")]
        public async Task<IActionResult> PickBookmark([FromBody] PickBookmarkRequest request)
        {
            try
            {
                var bookmark = await _sharedLibraryService.PickBookmarkAsync(UserId, request.PoolBookmarkId, request.CategoryId);
                return Ok(bookmark);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

    public record PickBookmarkRequest(Guid PoolBookmarkId, Guid CategoryId);
}
