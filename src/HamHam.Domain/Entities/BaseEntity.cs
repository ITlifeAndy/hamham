using System;

namespace HamHam.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public Guid CreatorUser { get; set; }
        public DateTime LastModifyTime { get; set; } = DateTime.UtcNow;
        public Guid LastModifyUser { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? DeleteUser { get; set; }
        public DateTime? DeleteTime { get; set; }
    }
}
