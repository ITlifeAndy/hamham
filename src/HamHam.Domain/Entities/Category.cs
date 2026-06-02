using System;

namespace HamHam.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid UsersId { get; set; }
        public Guid? CategoriesId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#dee1ff";
        public string? TextColor { get; set; }
        public string Icon { get; set; } = "folder";
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
