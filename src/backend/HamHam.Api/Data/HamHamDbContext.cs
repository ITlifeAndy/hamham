using Microsoft.EntityFrameworkCore;
using HamHam.Api.Models;

namespace HamHam.Api.Data
{
    public class HamHamDbContext : DbContext
    {
        public HamHamDbContext(DbContextOptions<HamHamDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPreferences> UserPreferences { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public DbSet<SharedPool> SharedPools { get; set; } = null!;
        public DbSet<SharedPoolBookmark> SharedPoolBookmarks { get; set; } = null!;
        public DbSet<IconLibrary> IconLibrary { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey<UserPreferences>(p => p.Users_Id);
        }
    }
}
