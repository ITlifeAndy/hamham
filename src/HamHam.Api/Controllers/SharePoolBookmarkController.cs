using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;

namespace HamHam.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/share-pool-bookmarks")]
    public class SharePoolBookmarkController : ControllerBase
    {
        private readonly ISharePoolBookmarkService _bookmarkService;

        public SharePoolBookmarkController(ISharePoolBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost]
        public async Task<ActionResult<SharedPoolBookmark>> CreateBookmark([FromBody] CreatePublicBookmarkRequest request)
        {
            var bookmark = await _bookmarkService.CreateBookmarkAsync(request.PoolId, request.Name, request.Url);
            return CreatedAtAction(nameof(GetBookmarks), new { poolId = request.PoolId }, bookmark);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SharedPoolBookmark>> UpdateBookmark(Guid id, [FromBody] UpdatePublicBookmarkRequest request)
        {
            try
            {
                var bookmark = await _bookmarkService.UpdateBookmarkAsync(id, request.Name, request.Url);
                return Ok(bookmark);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBookmark(Guid id)
        {
            var success = await _bookmarkService.DeleteBookmarkAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SharedPoolBookmark>>> GetBookmarks([FromQuery] Guid? poolId)
        {
            return Ok(await _bookmarkService.GetBookmarksAsync(poolId));
        }
    }

    public class CreatePublicBookmarkRequest
    {
        public Guid PoolId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class UpdatePublicBookmarkRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
