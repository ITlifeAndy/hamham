using System.Collections.Generic;
using System.Threading.Tasks;

namespace HamHam.Application.Interfaces
{
    public interface IBookmarkImportService
    {
        Task<bool> ImportPublicBookmarksAsync(string userId, List<string> publicBookmarkIds, string targetCategoryId, string color, bool glass);
    }
}
