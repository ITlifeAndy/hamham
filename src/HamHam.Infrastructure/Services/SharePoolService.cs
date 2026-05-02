using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using HamHam.Infrastructure.Persistence;

namespace HamHam.Infrastructure.Services
{
    public class SharePoolService : ISharePoolService
    {
        private readonly HamHamDbContext _context;

        public SharePoolService(HamHamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SharedPool>> GetPoolsAsync()
        {
            return await _context.SharedPools
                .Where(sp => !sp.IsDeleted)
                .ToListAsync();
        }

        public async Task<SharedPool> CreatePoolAsync(string name)
        {
            var pool = new SharedPool
            {
                Id = Guid.NewGuid(),
                Name = name,
                IsPublic = true,
                Description = string.Empty
            };

            _context.SharedPools.Add(pool);
            await _context.SaveChangesAsync();
            return pool;
        }

        public async Task<SharedPool> UpdatePoolAsync(Guid id, string name)
        {
            var pool = await _context.SharedPools
                .FirstOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);

            if (pool == null) throw new KeyNotFoundException("Share pool not found");

            pool.Name = name;
            await _context.SaveChangesAsync();
            return pool;
        }

        public async Task<bool> DeletePoolAsync(Guid id)
        {
            var pool = await _context.SharedPools
                .FirstOrDefaultAsync(sp => sp.Id == id && !sp.IsDeleted);

            if (pool == null) return false;

            pool.IsDeleted = true;
            
            // Mark all associated bookmarks as deleted
            var bookmarks = await _context.SharedPoolBookmarks
                .Where(b => b.SharedPoolsId == id)
                .ToListAsync();
            
            foreach (var b in bookmarks)
            {
                b.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
