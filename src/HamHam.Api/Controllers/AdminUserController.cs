using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HamHam.Api.Controllers
{
    public record CreateUserRequest(string Name, string Username, string Email, string Password, UserRole Role, bool IsActive);
    public record UpdateUserRequest(string Name, string Username, string Email, UserRole Role, bool IsActive);

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AdminUserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("sub")?.Value;
            if (userId == null) return Unauthorized();

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            return Ok(new { user.Id, user.Name, user.Role, user.IsActive });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] UserRole? role = null, [FromQuery] bool? isActive = null)
        {
            var (users, totalCount) = await _userService.GetUsersAsync(page, pageSize, searchTerm, role, isActive);
            return Ok(new { Users = users, TotalCount = totalCount });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null) return BadRequest();
            
            var passwordHash = await _authService.HashPassword(request.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role,
                IsActive = request.IsActive
            };

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.Name = request.Name;
            user.Username = request.Username;
            user.Email = request.Email;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            var updatedUser = await _userService.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
