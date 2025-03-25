using Microsoft.EntityFrameworkCore;
using SportifyApi.Models;

namespace SportifyApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}


// STEPS TO CREATE A TABLE IN MY DB:
// 1. Create a model to represent the structure/columns of your table
// 2. Add the table in our db context
// 3. Create a migration specifying the changes made (dotnet ef migrations add InitialCreation)
// 4. database update to sync (dotnet ef database update )
