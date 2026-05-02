using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HamHam.Infrastructure.Persistence;
using HamHam.Domain.Entities;
using BCrypt.Net;

namespace HamHam.Infrastructure.Services
{
    public class UserSeeder
    {
        private readonly HamHamDbContext _context;

        public UserSeeder(HamHamDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync())
            {
                return;
            }

            try
            {
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@hamham.local",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("hamham"),
                    Role = UserRole.Admin,
                    IsActive = true
                };

                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle concurrent creation attempts: if the user already exists due to 
                // another instance creating it, ignore the exception.
            }
        }

    }
}
