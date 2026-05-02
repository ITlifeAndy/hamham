using System.Collections.Generic;
using System.Threading.Tasks;

namespace HamHam.Application.Interfaces
{
    public interface IUnsplashService
    {
        Task<List<string>> GetRandomWallpapersAsync(string[] keywords);
    }
}
