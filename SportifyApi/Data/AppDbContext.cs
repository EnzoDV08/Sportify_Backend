using Microsoft.EntityFrameworkCore;
using SportifyApi.Models;

namespace SportifyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // ✅ DbSet declarations
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationProfile> OrganizationProfiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Profile → Primary Key (UserId)
            modelBuilder.Entity<Profile>()
                .HasKey(p => p.UserId);

            // ✅ Event ↔ Participants (many-to-one, navigates to Event.Participants)
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Participant ↔ User (many-to-one, no reverse nav)
            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany() // No navigation from User → EventParticipant
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ User ↔ UserAchievements (one-to-many)
            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.User)
                .WithMany() // No navigation from User → UserAchievement
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Achievement ↔ UserAchievement (one-to-many)
            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Achievement)
                .WithMany(a => a.UserAchievements)
                .HasForeignKey(ua => ua.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Event ↔ UserAchievement (nullable FK, e.g., "First Join" is not tied to a specific event)
            modelBuilder.Entity<UserAchievement>()
                .HasOne(ua => ua.Event)
                .WithMany()
                .HasForeignKey(ua => ua.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Organization>().HasKey(o => o.OrganizationId);

            modelBuilder.Entity<OrganizationProfile>()
                .HasKey(op => op.OrganizationId);

            modelBuilder.Entity<Achievement>()
                .HasKey(a => a.AchievementId);


        }
    }
}
