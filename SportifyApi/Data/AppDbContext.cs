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
        public DbSet<Friend> Friends { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Profile>()
                .HasKey(p => p.UserId);


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
                
            modelBuilder.Entity<Event>()
                .Property(e => e.InvitedUserIds)
                .HasColumnType("integer[]");

        }
    }
}