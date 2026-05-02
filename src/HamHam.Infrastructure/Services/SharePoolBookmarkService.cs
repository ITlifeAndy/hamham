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
    public class SharePoolBookmarkService : ISharePoolBookmarkService
    {
        private readonly HamHamDbContext _context;

        public SharePoolBookmarkService(HamHamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SharedPoolBookmark>> GetBookmarksAsync(Guid? poolId)
        {
            var query = _context.SharedPoolBookmarks.Where(b => !b.IsDeleted);
            
            if (poolId.HasValue)
            {
                query = query.Where(b => b.SharedPoolsId == poolId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<SharedPoolBookmark> CreateBookmarkAsync(Guid poolId, string name, string url)
        {
            var bookmark = new SharedPoolBookmark
            {
                Id = Guid.NewGuid(),
                SharedPoolsId = poolId,
                Name = name,
                Url = url
            };

            _context.SharedPoolBookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return bookmark;
        }

        public async Task<SharedPoolBookmark> UpdateBookmarkAsync(Guid id, string name, string url)
        {
            var bookmark = await _context.SharedPoolBookmarks
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (bookmark == null) throw new KeyNotFoundException("Public bookmark not found");

            bookmark.Name = name;
            bookmark.Url = url;
            await _context.SaveChangesAsync();
            return bookmark;
        }

        public async Task<bool> DeleteBookmarkAsync(Guid id)
        {
            var bookmark = await _context.SharedPoolBookmarks
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            if (bookmark == null) return false;

            bookmark.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
