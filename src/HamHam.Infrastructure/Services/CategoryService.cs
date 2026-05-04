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
    public class CategoryService : ICategoryService
    {
        private readonly HamHamDbContext _context;
        private readonly IHubContext<SyncHub> _hubContext;

        public CategoryService(HamHamDbContext context, IHubContext<SyncHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Guid userId)
        {
            return await _context.Categories
                .Where(c => c.UsersId == userId && !c.IsDeleted)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<UnifiedItem>> GetUnifiedItemsAsync(Guid userId, Guid categoryId)
        {
            var categoryIds = new List<Guid> { categoryId };
            var allChildren = new List<Category>();
            await GetChildrenRecursive(categoryId, allChildren);
            categoryIds.AddRange(allChildren.Select(c => c.Id));

            var bookmarks = await _context.Bookmarks
                .Where(b => b.UsersId == userId && categoryIds.Contains(b.CategoriesId) && !b.IsDeleted)
                .OrderBy(b => b.SortOrder)
                .ThenBy(b => b.Id)
                .ToListAsync();


            var subCategories = await _context.Categories
                .Where(c => c.CategoriesId == categoryId && !c.IsDeleted)
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.Id)
                .ToListAsync();

            var result = new List<UnifiedItem>();

            foreach (var bm in bookmarks)
            {
                result.Add(new UnifiedItem(bm.Id, "Bookmark", bm.SortOrder, bm));
            }
            foreach (var cat in subCategories)
            {
                result.Add(new UnifiedItem(cat.Id, "Category", cat.SortOrder, cat));
            }

            return result;
        }

        public async Task<Category> CreateCategoryAsync(Guid userId, CreateCategoryRequest request)
        {
            var maxSortOrder = await _context.Categories
                .Where(c => c.UsersId == userId && c.CategoriesId == request.ParentId)
                .MaxAsync(c => (int?)c.SortOrder) ?? -1;

            var category = new Category
            {
                Id = Guid.NewGuid(),
                UsersId = userId,
                Name = request.Name,
                Color = request.Color,
                CategoriesId = request.ParentId,
                Icon = request.Icon,
                SortOrder = maxSortOrder + 1
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyCategoryChanged", new CategoryEvent("CREATE", category.Id, userId));
            
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UsersId == userId);

            if (category == null) throw new KeyNotFoundException("Category not found");

            category.Name = request.Name;
            category.Color = request.Color;
            category.CategoriesId = request.ParentId;
            category.Icon = request.Icon;
            if (request.SortOrder.HasValue)
            {
                category.SortOrder = request.SortOrder.Value;
            }

            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyCategoryChanged", new CategoryEvent("UPDATE", category.Id, userId));
            
            return category;
        }

        public async Task<bool> DeleteCategoryRecursiveAsync(Guid userId, Guid categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UsersId == userId);

            if (category == null) return false;

            var allChildren = new List<Category>();
            await GetChildrenRecursive(categoryId, allChildren);

            category.IsDeleted = true;
            foreach (var child in allChildren)
            {
                child.IsDeleted = true;
            }

            var categoryIds = allChildren.Select(c => c.Id).Append(categoryId).ToList();
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UsersId == userId && categoryIds.Contains(b.CategoriesId))
                .ToListAsync();

            foreach (var b in bookmarks)
            {
                b.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyCategoryChanged", new CategoryEvent("DELETE", categoryId, userId));
            
            return true;
        }

        public async Task UpdateCategoryOrderAsync(Guid userId, IEnumerable<CategoryOrderRequest> orders)
        {
            var categoryIds = orders.Select(o => o.CategoryId).ToList();
            var categories = await _context.Categories
                .Where(c => c.UsersId == userId && categoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var order in orders)
            {
                var category = categories.FirstOrDefault(c => c.Id == order.CategoryId);
                if (category != null)
                {
                    category.SortOrder = order.SortOrder;
                }
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyCategoryChanged", new CategoryEvent("UPDATE_ORDER", Guid.Empty, userId));
        }

        public async Task UpdateUnifiedOrderAsync(Guid userId, Guid categoryId, IEnumerable<UnifiedOrderRequest> orders)
        {
            foreach (var order in orders)
            {
                if (order.Type == "Bookmark")
                {
                    var bookmark = await _context.Bookmarks
                        .FirstOrDefaultAsync(b => b.Id == order.ItemId && b.UsersId == userId);
                    if (bookmark != null)
                    {
                        bookmark.SortOrder = order.SortOrder;
                    }
                }
                else if (order.Type == "Category")
                {
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Id == order.ItemId && c.UsersId == userId);
                    if (category != null)
                    {
                        category.SortOrder = order.SortOrder;
                    }
                }
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group(userId.ToString()).SendAsync("NotifyCategoryChanged", new CategoryEvent("UPDATE_ORDER", categoryId, userId));
        }

        private async Task GetChildrenRecursive(Guid parentId, List<Category> result)
        {
            var children = await _context.Categories
                .Where(c => c.CategoriesId == parentId && !c.IsDeleted)
                .ToListAsync();

            foreach (var child in children)
            {
                result.Add(child);
                await GetChildrenRecursive(child.Id, result);
            }
        }
    }
}
