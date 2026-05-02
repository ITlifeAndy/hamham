using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using HamHam.Domain.Events;
using HamHam.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using HamHam.Infrastructure.Hubs;

namespace HamHam.Infrastructure.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly HamHamDbContext _context;
        private readonly IHubContext<SyncHub> _hubContext;

        public BookmarkService(HamHamDbContext context, IHubContext<SyncHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Bookmark>> GetBookmarksAsync(Guid userId, Guid? categoryId = null)
        {
            var query = _context.Bookmarks.Where(b => b.UsersId == userId);
            if (categoryId.HasValue)
            {
                var categoryIds = new List<Guid> { categoryId.Value };
                await GetCategoryChildrenRecursive(categoryId.Value, categoryIds);
                query = query.Where(b => categoryIds.Contains(b.CategoriesId));
            }
            return await query.OrderBy(b => b.SortOrder).ToListAsync();
        }

        private async Task GetCategoryChildrenRecursive(Guid parentId, List<Guid> result)
        {
            var children = await _context.Categories
                .Where(c => c.CategoriesId == parentId && !c.IsDeleted)
                .Select(c => c.Id)
                .ToListAsync();

            foreach (var childId in children)
            {
                result.Add(childId);
                await GetCategoryChildrenRecursive(childId, result);
            }
        }

        public async Task<Bookmark> CreateBookmarkAsync(Guid userId, CreateBookmarkRequest request)
        {
            Guid finalCategoryId = request.categoryId ?? Guid.Empty;
            if (finalCategoryId == Guid.Empty)
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.UsersId == userId && c.CategoriesId == null);

                if (rootCategory == null)
                {
                    rootCategory = new Category { Id = Guid.NewGuid(), UsersId = userId, Name = "Default", Color = "#FFFFFF", CategoriesId = null };
                    _context.Categories.Add(rootCategory);
                    await _context.SaveChangesAsync();
                }
                finalCategoryId = rootCategory.Id;
            }

            var bookmark = new Bookmark
            {
                Id = Guid.NewGuid(),
                UsersId = userId,
                Title = request.Title,
                Subtitle = request.Subtitle,
                Url = request.Url,
                CategoriesId = finalCategoryId,
                Icon = request.icon,
                Color = request.color,
                SortOrder = 0
            };

            _context.Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyBookmarkChanged", new BookmarkEvent("CREATE", bookmark.Id, userId));
            
            return bookmark;
        }

        public async Task<Bookmark> UpdateBookmarkAsync(Guid userId, Guid bookmarkId, UpdateBookmarkRequest request)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.Id == bookmarkId && b.UsersId == userId);

            if (bookmark == null) throw new KeyNotFoundException("Bookmark not found");

            Guid finalCategoryId = request.categoryId ?? Guid.Empty;
            if (finalCategoryId == Guid.Empty)
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.UsersId == userId && c.CategoriesId == null);

                if (rootCategory == null)
                {
                    rootCategory = new Category { Id = Guid.NewGuid(), UsersId = userId, Name = "Default", Color = "#FFFFFF", CategoriesId = null };
                    _context.Categories.Add(rootCategory);
                    await _context.SaveChangesAsync();
                }
                finalCategoryId = rootCategory.Id;
            }

            bookmark.Title = request.Title;
            bookmark.Subtitle = request.Subtitle;
            bookmark.Url = request.Url;
            bookmark.CategoriesId = finalCategoryId;
            bookmark.Icon = request.icon;
            bookmark.Color = request.color;
            bookmark.IsFavorite = request.IsFavorite;

            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyBookmarkChanged", new BookmarkEvent("UPDATE", bookmark.Id, userId));
            
            return bookmark;
        }

        public async Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookmarkId)
        {
            var bookmark = await _context.Bookmarks
                .FirstOrDefaultAsync(b => b.Id == bookmarkId && b.UsersId == userId);

            if (bookmark == null) return false;

            bookmark.IsDeleted = true;
            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyBookmarkChanged", new BookmarkEvent("DELETE", bookmarkId, userId));
            
            return true;
        }

        public async Task UpdateBookmarkOrderAsync(Guid userId, List<Guid> bookmarkIds)
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UsersId == userId && bookmarkIds.Contains(b.Id))
                .ToListAsync();

            for (int i = 0; i < bookmarkIds.Count; i++)
            {
                var bookmark = bookmarks.FirstOrDefault(b => b.Id == bookmarkIds[i]);
                if (bookmark != null)
                {
                    bookmark.SortOrder = i;
                }
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyBookmarkChanged", new BookmarkEvent("UPDATE_ORDER", Guid.Empty, userId));
        }

        public async Task<int> ImportBookmarksAsync(Guid userId, List<ImportBookmarkRequest> bookmarks)
        {
            int count = 0;
            foreach (var bReq in bookmarks)
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.UsersId == userId && c.CategoriesId == null);
                
                if (rootCategory == null)
                {
                    rootCategory = new Category { Id = Guid.NewGuid(), UsersId = userId, Name = "Imported", Color = "#FFFFFF", CategoriesId = null };
                    _context.Categories.Add(rootCategory);
                    await _context.SaveChangesAsync();
                }

                var bookmark = new Bookmark
                {
                    Id = Guid.NewGuid(),
                    UsersId = userId,
                    Title = bReq.Title,
                    Url = bReq.Url,
                    CategoriesId = rootCategory.Id
                };
                _context.Bookmarks.Add(bookmark);
                count++;
            }

            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyBookmarkChanged", new BookmarkEvent("IMPORT", Guid.Empty, userId));
            
            return count;
        }
    }
}
