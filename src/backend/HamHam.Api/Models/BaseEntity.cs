using System;

namespace HamHam.Api.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        // Auditing
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public Guid CreatorUser { get; set; }
        public DateTime LastModifyTime { get; set; } = DateTime.UtcNow;
        public Guid LastModifyUser { get; set; }
        
        // Soft Delete
        public bool IsDeleted { get; set; } = false;
        public Guid? DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
