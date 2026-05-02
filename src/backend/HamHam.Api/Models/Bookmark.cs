using System;

namespace HamHam.Api.Models
{
    public class Bookmark : BaseEntity
    {
        public Guid Users_Id { get; set; }
        public Guid Categories_Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string FaviconUrl { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
        public Guid? IconLibrary_Id { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual IconLibrary Icon { get; set; } = null!;
    }
}
