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
        }
    }
}

// STEPS TO CREATE A TABLE IN MY DB:
// 1. Create a model to represent the structure/columns of your table
// 2. Add the table in our db context
// 3. Create a migration specifying the changes made (dotnet ef migrations add InitialCreation)
// 4. database update to sync (dotnet ef database update )
