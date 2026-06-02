using System;

namespace HamHam.Domain.Entities
{
    public class Bookmark : BaseEntity
    {
        public Guid UsersId { get; set; }
        public Guid CategoriesId { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? TextColor { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string Url { get; set; } = string.Empty;
        public string FaviconUrl { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
    }
}
