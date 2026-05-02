using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using HamHam.Application.Interfaces;
using HamHam.Domain.Entities;
using HamHam.Infrastructure.Persistence;

namespace HamHam.Infrastructure.Services
{
    public class IconLibraryService : IIconLibraryService
    {
        private readonly HamHamDbContext _context;
        private readonly string _storagePath = "uploads/icons";

        public IconLibraryService(HamHamDbContext context)
        {
            _context = context;
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<IEnumerable<IconLibrary>> ListIconsAsync(string category = null)
        {
            var query = _context.IconLibrary.AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(i => i.Category == category);
            }
            return await query.ToListAsync();
        }

        public async Task<IconLibrary> UploadIconAsync(Guid userId, string name, string category, byte[] fileContent, string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var fileNameUnique = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_storagePath, fileNameUnique);

            await File.WriteAllBytesAsync(filePath, fileContent);

            var icon = new IconLibrary
            {
                Id = Guid.NewGuid(),
                Name = name,
                Category = category,
                FilePath = filePath
            };

            _context.IconLibrary.Add(icon);
            await _context.SaveChangesAsync();
            return icon;
        }

        public async Task<bool> DeleteIconAsync(Guid userId, Guid iconId)
        {
            var icon = await _context.IconLibrary.FindAsync(iconId);
            if (icon == null) return false;

            // In a real app, verify userId owns the icon or is admin
            
            icon.IsDeleted = true;
            
            if (File.Exists(icon.FilePath))
            {
                File.Delete(icon.FilePath);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
