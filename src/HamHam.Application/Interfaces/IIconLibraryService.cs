using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HamHam.Domain.Entities;

namespace HamHam.Application.Interfaces
{
    public interface IIconLibraryService
    {
        Task<IEnumerable<IconLibrary>> ListIconsAsync(string category = null);
        Task<IconLibrary> UploadIconAsync(Guid userId, string name, string category, byte[] fileContent, string fileName);
        Task<bool> DeleteIconAsync(Guid userId, Guid iconId);
    }
}
