using System;

namespace HamHam.Domain.Entities
{
    public class IconLibrary : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., "work", "social"
    }
}
