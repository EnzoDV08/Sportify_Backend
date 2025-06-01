using Microsoft.EntityFrameworkCore;
using SportifyApi.Models;

namespace SportifyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // DbSet properties for each model/table in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationProfile> OrganizationProfiles { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Profile>()
                // Makes sure that userID is the primary key in the Profile table
                .HasKey(p => p.UserId);

            modelBuilder.Entity<Organization>().HasKey(o => o.OrganizationId);

            modelBuilder.Entity<OrganizationProfile>()
                .HasKey(op => op.OrganizationId);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithMany()
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