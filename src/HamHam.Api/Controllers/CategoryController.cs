using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync(UserId);
            return Ok(categories);
        }

        [HttpGet("unified/{categoryId}")]
        public async Task<IActionResult> GetUnifiedItems(Guid categoryId)
        {
            var items = await _categoryService.GetUnifiedItemsAsync(UserId, categoryId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateCategoryAsync(UserId, request);
            return Ok(category);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateCategoryAsync(UserId, id, request);
            return Ok(category);
        }

        [HttpPatch("order")]
        public async Task<IActionResult> UpdateCategoryOrder([FromBody] IEnumerable<CategoryOrderRequest> orders)
        {
            await _categoryService.UpdateCategoryOrderAsync(UserId, orders);
            return Ok();
        }

        [HttpPatch("unified-order")]
        public async Task<IActionResult> UpdateUnifiedOrder(Guid categoryId, [FromBody] IEnumerable<UnifiedOrderRequest> orders)
        {
            await _categoryService.UpdateUnifiedOrderAsync(UserId, categoryId, orders);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var success = await _categoryService.DeleteCategoryRecursiveAsync(UserId, id);
            return success ? Ok() : NotFound();
        }
    }
}
