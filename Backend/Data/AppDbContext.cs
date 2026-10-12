using Microsoft.EntityFrameworkCore;
using ServerCSharp.Models;

namespace ServerCSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryGenre> StoryGenres { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ReadingHistory> ReadingHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for StoryGenre
            modelBuilder.Entity<StoryGenre>()
                .HasKey(sg => new { sg.StoryId, sg.GenreId });

            modelBuilder.Entity<StoryGenre>()
                .HasOne(sg => sg.Story)
                .WithMany(s => s.StoryGenres)
                .HasForeignKey(sg => sg.StoryId);

            modelBuilder.Entity<StoryGenre>()
                .HasOne(sg => sg.Genre)
                .WithMany(g => g.StoryGenres)
                .HasForeignKey(sg => sg.GenreId);

            // Unique constraint on User Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Unique constraint for Favorite
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.StoryId })
                .IsUnique();

            // Unique constraint for ReadingHistory
            modelBuilder.Entity<ReadingHistory>()
                .HasIndex(h => new { h.UserId, h.StoryId })
                .IsUnique();
        }
    }
}
