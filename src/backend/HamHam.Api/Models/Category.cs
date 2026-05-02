using System;

namespace HamHam.Api.Models
{
    public class Category : BaseEntity
    {
        public Guid Users_Id { get; set; }
        public Guid? Categories_Id { get; set; } // Parent Category
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public Guid? IconLibrary_Id { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual Category ParentCategory { get; set; } = null!;
        public virtual IconLibrary Icon { get; set; } = null!;
    }
}
