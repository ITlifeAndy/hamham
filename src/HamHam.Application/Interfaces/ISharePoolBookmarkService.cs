using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface ISharePoolBookmarkService
    {
        Task<IEnumerable<SharedPoolBookmark>> GetBookmarksAsync(Guid? poolId);
        Task<SharedPoolBookmark> CreateBookmarkAsync(Guid poolId, string name, string url);
        Task<SharedPoolBookmark> UpdateBookmarkAsync(Guid id, string name, string url);
        Task<bool> DeleteBookmarkAsync(Guid id);
    }
}
