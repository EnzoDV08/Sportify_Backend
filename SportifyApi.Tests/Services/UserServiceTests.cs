using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Services;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class UserServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task CreateUserAsync_CreatesUserAndProfile()
        {
            var context = await GetDbContextAsync();
            var service = new UserService(context);

            var dto = new UserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "test123",
                UserType = "user"
            };

            var result = await service.CreateUserAsync(dto, "test123");

            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
            var profile = await context.Profiles.FirstOrDefaultAsync(p => p.UserId == result.UserId);
            Assert.NotNull(profile);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenExists()
        {
            var context = await GetDbContextAsync();
            var service = new UserService(context);

            var user = await service.CreateUserAsync(new UserDto
            {
                Name = "Fetch User",
                Email = "fetch@example.com",
                Password = "123"
            }, "123");

            var fetched = await service.GetUserByIdAsync(user.UserId);

            Assert.NotNull(fetched);
            Assert.Equal("Fetch User", fetched.Name);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesFields()
        {
            var context = await GetDbContextAsync();
            var service = new UserService(context);

            var created = await service.CreateUserAsync(new UserDto
            {
                Name = "Old Name",
                Email = "old@example.com",
                Password = "pass"
            }, "pass");

            created.Name = "New Name";
            created.Email = "new@example.com";
            var updated = await service.UpdateUserAsync(created.UserId, created);

            Assert.True(updated);
            var user = await context.Users.FindAsync(created.UserId);
            Assert.Equal("New Name", user!.Name);
        }

        [Fact]
        public async Task DeleteUserAsync_RemovesUser()
        {
            var context = await GetDbContextAsync();
            var service = new UserService(context);

            var user = await service.CreateUserAsync(new UserDto
            {
                Name = "Delete Me",
                Email = "delete@example.com",
                Password = "pass"
            }, "pass");

            var deleted = await service.DeleteUserAsync(user.UserId);

            Assert.True(deleted);
            var userCheck = await context.Users.FindAsync(user.UserId);
            Assert.Null(userCheck);
        }
    }
}