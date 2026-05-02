using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using HamHam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HamHam.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HamHamDbContext _context;

        public UserService(HamHamDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string searchTerm, UserRole? role = null, bool? isActive = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => u.Name.Contains(searchTerm) || 
                                         u.Email.Contains(searchTerm) || 
                                         u.Username.Contains(searchTerm));
            }

            if (role.HasValue)
            {
                query = query.Where(u => u.Role == role.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            int totalCount = await query.CountAsync();
            var users = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return (users, totalCount);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
