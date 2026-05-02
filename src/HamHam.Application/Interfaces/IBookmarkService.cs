using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface IBookmarkService
    {
        Task<IEnumerable<Bookmark>> GetBookmarksAsync(Guid userId, Guid? categoryId = null);
        Task<Bookmark> CreateBookmarkAsync(Guid userId, CreateBookmarkRequest request);
        Task<Bookmark> UpdateBookmarkAsync(Guid userId, Guid bookmarkId, UpdateBookmarkRequest request);
        Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookmarkId);
        Task<int> ImportBookmarksAsync(Guid userId, List<ImportBookmarkRequest> bookmarks);
        Task UpdateBookmarkOrderAsync(Guid userId, List<Guid> bookmarkIds);
    }

    public record CreateBookmarkRequest(string Title, string? Subtitle, string Url, Guid? categoryId, string? icon, string? color);
    public record UpdateBookmarkRequest(string Title, string? Subtitle, string Url, Guid? categoryId, string? icon, string? color, bool IsFavorite);
    public record ImportBookmarkRequest(string Title, string Url, string FolderPath);
}
