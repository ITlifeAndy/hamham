using System;

namespace HamHam.Domain.Entities
{
    public class SharedPoolBookmark : BaseEntity
    {
        public Guid SharedPoolsId { get; set; }
        public string? Icon { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; } // Internal pool category
    }
}
