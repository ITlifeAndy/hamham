using System;

namespace HamHam.Api.Models
{
    public class SharedPool : BaseEntity
    {
        public Guid Users_Id { get; set; } // Creator
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = true;
        public Guid? IconLibrary_Id { get; set; }

        // Navigation Properties
        public virtual User Creator { get; set; } = null!;
        public virtual IconLibrary Icon { get; set; } = null!;
    }
}
