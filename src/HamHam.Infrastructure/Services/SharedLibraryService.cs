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
    public class SharedLibraryService : ISharedLibraryService
    {
        private readonly HamHamDbContext _context;

        public SharedLibraryService(HamHamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SharedPool>> GetPublicPoolsAsync()
        {
            return await _context.SharedPools
                .Where(p => p.IsPublic)
                .ToListAsync();
        }

        public async Task<SharedPool> GetPoolByIdAsync(Guid poolId)
        {
            return await _context.SharedPools
                .FirstOrDefaultAsync(p => p.Id == poolId);
        }

        public async Task<IEnumerable<SharedPoolBookmark>> GetPoolBookmarksAsync(Guid poolId)
        {
            return await _context.SharedPoolBookmarks
                .Where(sb => sb.SharedPoolsId == poolId)
                .ToListAsync();
        }

        public async Task<SharedPool> CreatePoolAsync(Guid userId, CreatePoolRequest request)
        {
            var pool = new SharedPool
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                IsPublic = request.IsPublic,
                Icon = request.Icon
            };

            _context.SharedPools.Add(pool);
            await _context.SaveChangesAsync();
            return pool;
        }

        public async Task<bool> UpdatePoolAsync(Guid userId, Guid poolId, UpdatePoolRequest request)
        {
            var pool = await _context.SharedPools
                .FirstOrDefaultAsync(p => p.Id == poolId && p.CreatorUser == userId);

            if (pool == null) return false;

            pool.Name = request.Name;
            pool.Description = request.Description;
            pool.IsPublic = request.IsPublic;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePoolAsync(Guid userId, Guid poolId)
        {
            var pool = await _context.SharedPools
                .FirstOrDefaultAsync(p => p.Id == poolId && p.CreatorUser == userId);

            if (pool == null) return false;

            pool.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Bookmark> PickBookmarkAsync(Guid userId, Guid poolBookmarkId, Guid targetCategoryId)
        {
            var poolBookmark = await _context.SharedPoolBookmarks
                .FirstOrDefaultAsync(sb => sb.Id == poolBookmarkId);

            if (poolBookmark == null) throw new KeyNotFoundException("Shared bookmark not found");

            var bookmark = new Bookmark
            {
                Id = Guid.NewGuid(),
                UsersId = userId,
                Title = poolBookmark.Name,
                Url = poolBookmark.Url,
                CategoriesId = targetCategoryId,
                Icon = poolBookmark.Icon
            };

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return bookmark;
        }
    }
}
