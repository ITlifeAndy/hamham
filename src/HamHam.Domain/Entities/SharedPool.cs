using System;

namespace HamHam.Domain.Entities
{
    public class SharedPool : BaseEntity
    {
        public string? Icon { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
    }
}
