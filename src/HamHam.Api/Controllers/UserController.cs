using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HamHam.Application.Interfaces;
using HamHam.Infrastructure.Persistence;
using HamHam.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HamHam.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly HamHamDbContext _context;
        private readonly IAuthService _authService;

        public UserController(HamHamDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
            {
                return BadRequest("Username or email already exists.");
            }

            var passwordHash = await _authService.HashPassword(request.Password);
            var user = new User
            {
                Name = request.Name,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = UserRole.User,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);
            if (user == null || !await _authService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!user.IsActive)
            {
                return Unauthorized("Account is disabled.");
            }

            var token = await _authService.GenerateJwtToken(user);
            var refreshToken = await _authService.GenerateRefreshToken();

            return Ok(new { Token = token, RefreshToken = refreshToken, Name = user.Name });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("admin/users/{id}/status")]
        public async Task<IActionResult> ToggleUserStatus(Guid id, [FromBody] StatusRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = request.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"User status updated to {user.IsActive}." });
        }
    }

    public record RegisterRequest(string Name, string Username, string Email, string Password);
    public record LoginRequest(string UsernameOrEmail, string Password);
    public record StatusRequest(bool IsActive);
}

