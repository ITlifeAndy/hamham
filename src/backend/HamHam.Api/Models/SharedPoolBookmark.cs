using System;

namespace HamHam.Api.Models
{
    public class SharedPoolBookmark : BaseEntity
    {
        public Guid SharedPools_Id { get; set; }
        public string BookmarkTitle { get; set; } = string.Empty;
        public string BookmarkUrl { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; } // Internal pool category
        public Guid? IconLibrary_Id { get; set; }

        // Navigation Properties
        public virtual SharedPool Pool { get; set; } = null!;
        public virtual IconLibrary Icon { get; set; } = null!;
    }
}
