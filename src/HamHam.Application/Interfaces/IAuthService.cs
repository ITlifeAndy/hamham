using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> HashPassword(string password);
        Task<bool> VerifyPassword(string password, string hash);
        Task<string> GenerateJwtToken(User user);
        Task<string> GenerateRefreshToken();
        Task<bool> ValidateRefreshToken(string refreshToken);
    }
}
