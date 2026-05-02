using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/icons")]
    public class IconLibraryController : ControllerBase
    {
        public class IconUploadRequest
        {
            public IFormFile File { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
        }

        private readonly IIconLibraryService _iconService;

        public IconLibraryController(IIconLibraryService iconService)
        {
            _iconService = iconService;
        }

        private Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListIcons([FromQuery] string category)
        {
            var icons = await _iconService.ListIconsAsync(category);
            return Ok(icons);
        }

        [Authorize]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadIcon([FromForm] IconUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0) return BadRequest("File is required");

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);
            var fileContent = ms.ToArray();

            var icon = await _iconService.UploadIconAsync(UserId, request.Name, request.Category, fileContent, request.File.FileName);
            return Ok(icon);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIcon(Guid id)
        {
            var success = await _iconService.DeleteIconAsync(UserId, id);
            return success ? Ok() : NotFound();
        }
    }
}
