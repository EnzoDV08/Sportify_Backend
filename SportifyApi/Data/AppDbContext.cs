using Microsoft.EntityFrameworkCore;
using SportifyApi.Models;

namespace SportifyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Profile: Primary key is also foreign key (1-to-1 with User)
            modelBuilder.Entity<Profile>()
                .HasKey(p => p.UserId);

            // Admin → User (many-to-one or one-to-one depending on design)
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithMany() // or WithOne() if strictly 1-to-1
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany()
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany()
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Achievement)
                .WithMany(a => a.UserAchievements)
                .HasForeignKey(ua => ua.AchievementId);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Event)
                .WithMany()
                .HasForeignKey(ua => ua.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.AwardedByAdmin)
                .WithMany()
                .HasForeignKey(ua => ua.AwardedByAdminId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

// STEPS TO CREATE A TABLE IN MY DB:
// 1. Create a model to represent the structure/columns of your table
// 2. Add the table in our db context
// 3. Create a migration specifying the changes made (dotnet ef migrations add InitialCreation)
// 4. database update to sync (dotnet ef database update )
