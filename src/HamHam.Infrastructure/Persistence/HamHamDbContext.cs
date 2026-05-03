using Microsoft.EntityFrameworkCore;
using HamHam.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace HamHam.Infrastructure.Persistence
{
    public class HamHamDbContext : DbContext
    {
        public HamHamDbContext(DbContextOptions<HamHamDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPreference> UserPreferences { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public DbSet<SharedPool> SharedPools { get; set; } = null!;
        public DbSet<SharedPoolBookmark> SharedPoolBookmarks { get; set; } = null!;
        public DbSet<IconLibrary> IconLibrary { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasQueryFilter(u => !u.IsDeleted);
            });

            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UsersId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(up => !up.IsDeleted);
            });

             modelBuilder.Entity<Category>(entity =>
             {
                 entity.HasKey(e => e.Id);
                 entity.HasOne<User>().WithMany().HasForeignKey(e => e.UsersId);
                 entity.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoriesId);
                 entity.HasQueryFilter(c => !c.IsDeleted);
             });


             modelBuilder.Entity<Bookmark>(entity =>
             {
                 entity.HasKey(e => e.Id);
                 entity.HasOne<User>().WithMany().HasForeignKey(e => e.UsersId);
                 entity.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoriesId);
                 entity.HasQueryFilter(b => !b.IsDeleted);
             });


              modelBuilder.Entity<SharedPool>(entity =>
              {
                  entity.HasKey(e => e.Id);
                  entity.HasQueryFilter(sp => !sp.IsDeleted);
              });


               modelBuilder.Entity<SharedPoolBookmark>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.HasIndex(e => new { e.SharedPoolsId, e.Url }).IsUnique();
                   entity.HasOne<SharedPool>().WithMany().HasForeignKey(e => e.SharedPoolsId);
                   entity.HasQueryFilter(spb => !spb.IsDeleted);
               });



            modelBuilder.Entity<IconLibrary>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasQueryFilter(il => !il.IsDeleted);
            });
        }



        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                var now = DateTime.UtcNow;
                // In a real app, we'd get the current user ID from the IHttpContextAccessor
                var currentUserId = Guid.Empty; 

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreationTime = now;
                    entity.CreatorUser = currentUserId;
                }

                entity.LastModifyTime = now;
                entity.LastModifyUser = currentUserId;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
