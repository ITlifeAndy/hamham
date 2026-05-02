using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/bookmarks")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly ICategoryService _categoryService;

        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        private Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet]
        public async Task<IActionResult> GetBookmarks([FromQuery] Guid? categoryId)
        {
            var bookmarks = await _bookmarkService.GetBookmarksAsync(UserId, categoryId);
            return Ok(bookmarks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookmark([FromBody] CreateBookmarkRequest request)
        {
            var bookmark = await _bookmarkService.CreateBookmarkAsync(UserId, request);
            return Ok(bookmark);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookmark(Guid id, [FromBody] UpdateBookmarkRequest request)
        {
            var bookmark = await _bookmarkService.UpdateBookmarkAsync(UserId, id, request);
            return Ok(bookmark);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmark(Guid id)
        {
            var success = await _bookmarkService.DeleteBookmarkAsync(UserId, id);
            return success ? Ok() : NotFound();
        }

        [HttpPatch("order")]
        public async Task<IActionResult> UpdateOrder([FromBody] List<Guid> bookmarkIds)
        {
            await _bookmarkService.UpdateBookmarkOrderAsync(UserId, bookmarkIds);
            return Ok();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportBookmarks([FromBody] List<ImportBookmarkRequest> bookmarks)
        {
            var count = await _bookmarkService.ImportBookmarksAsync(UserId, bookmarks);
            return Ok(new { ImportedCount = count });
        }
    }
}
