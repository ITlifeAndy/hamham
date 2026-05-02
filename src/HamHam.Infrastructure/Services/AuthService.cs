using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;

namespace HamHam.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> HashPassword(string password)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
        }

        public Task<bool> VerifyPassword(string password, string hash)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hash));
        }

        public Task<string> GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "super_secret_key_for_hamham_1234567890"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("IsActive", user.IsActive.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<string> GenerateRefreshToken()
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<bool> ValidateRefreshToken(string refreshToken)
        {
            // In a real app, this would check against the DB/Redis.
            return Task.FromResult(!string.IsNullOrEmpty(refreshToken));
        }
    }
}
