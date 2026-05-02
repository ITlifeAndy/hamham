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
    [Route("api/share-pools")]
    public class SharePoolController : ControllerBase
    {
        private readonly ISharePoolService _poolService;

        public SharePoolController(ISharePoolService poolService)
        {
            _poolService = poolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SharedPool>>> GetPools()
        {
            return Ok(await _poolService.GetPoolsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<SharedPool>> CreatePool([FromBody] CreatePublicPoolRequest request)
        {
            var pool = await _poolService.CreatePoolAsync(request.Name);
            return CreatedAtAction(nameof(GetPools), new { id = pool.Id }, pool);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SharedPool>> UpdatePool(Guid id, [FromBody] UpdatePublicPoolRequest request)
        {
            try
            {
                var pool = await _poolService.UpdatePoolAsync(id, request.Name);
                return Ok(pool);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePool(Guid id)
        {
            var success = await _poolService.DeletePoolAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

    public class CreatePublicPoolRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdatePublicPoolRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
