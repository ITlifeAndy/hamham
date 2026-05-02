using System;

namespace HamHam.Api.Models
{
    public class IconLibrary : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
