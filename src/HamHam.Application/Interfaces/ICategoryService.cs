using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(Guid userId);
        Task<IEnumerable<UnifiedItem>> GetUnifiedItemsAsync(Guid userId, Guid categoryId);
        Task<Category> CreateCategoryAsync(Guid userId, CreateCategoryRequest request);
        Task<Category> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request);
        Task<bool> DeleteCategoryRecursiveAsync(Guid userId, Guid categoryId);
        Task UpdateUnifiedOrderAsync(Guid userId, Guid categoryId, IEnumerable<UnifiedOrderRequest> orders);
        Task UpdateCategoryOrderAsync(Guid userId, IEnumerable<CategoryOrderRequest> orders);
    }

    public record CreateCategoryRequest(string Name, string Color, Guid? ParentId, string? Icon, string? TextColor);
    public record UpdateCategoryRequest(string Name, string Color, Guid? ParentId, string? Icon, int? SortOrder, string? TextColor);
    public record UnifiedOrderRequest(Guid ItemId, string Type, int SortOrder);
    public record CategoryOrderRequest(Guid CategoryId, int SortOrder);
    public record UnifiedItem(Guid ItemId, string Type, int SortOrder, object Data);
}
