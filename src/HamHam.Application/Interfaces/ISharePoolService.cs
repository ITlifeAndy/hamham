using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface ISharePoolService
    {
        Task<IEnumerable<SharedPool>> GetPoolsAsync();
        Task<SharedPool> CreatePoolAsync(string name);
        Task<SharedPool> UpdatePoolAsync(Guid id, string name);
        Task<bool> DeletePoolAsync(Guid id);
    }
}
