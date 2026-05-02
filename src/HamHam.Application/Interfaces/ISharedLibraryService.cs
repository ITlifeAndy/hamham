using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface ISharedLibraryService
    {
        Task<IEnumerable<SharedPool>> GetPublicPoolsAsync();
        Task<SharedPool> GetPoolByIdAsync(Guid poolId);
        Task<IEnumerable<SharedPoolBookmark>> GetPoolBookmarksAsync(Guid poolId);
        Task<SharedPool> CreatePoolAsync(Guid userId, CreatePoolRequest request);
        Task<bool> UpdatePoolAsync(Guid userId, Guid poolId, UpdatePoolRequest request);
        Task<bool> DeletePoolAsync(Guid userId, Guid poolId);
        Task<Bookmark> PickBookmarkAsync(Guid userId, Guid poolBookmarkId, Guid targetCategoryId);
    }

    public record CreatePoolRequest(string Name, string Description, bool IsPublic, string? Icon);
    public record UpdatePoolRequest(string Name, string Description, bool IsPublic);
}
