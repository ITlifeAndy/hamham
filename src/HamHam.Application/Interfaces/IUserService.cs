using HamHam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HamHam.Application.Interfaces
{
    public interface IUserService
    {
        Task<(IEnumerable<User> Users, int TotalCount)> GetUsersAsync(int page, int pageSize, string searchTerm, UserRole? role = null, bool? isActive = null);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
