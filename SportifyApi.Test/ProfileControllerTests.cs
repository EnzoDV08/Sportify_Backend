using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Models;
using SportifyApi.Services;
using Xunit;

namespace SportifyApi.Test
{
    public class ProfileServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ProfileTestDb")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetProfileByIdAsync_ShouldReturnProfile_WhenExists()
        {
            // Arrange
            var context = GetDbContext();

            var user = new User
            {
                UserId = 1,
                Name = "Test User",
                Email = "test@example.com",
                Password = "secret",
                UserType = "user"
            };

            var profile = new Profile
            {
                UserId = 1,
                ProfilePicture = "test-pic.jpg",
                Bio = "Hello, I’m testing!",
                Location = "Cape Town"
            };

            context.Users.Add(user);
            context.Profiles.Add(profile);
            await context.SaveChangesAsync();

            var service = new ProfileService(context);

            // Act
            var result = await service.GetProfileByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cape Town", result.Location);
            Assert.Equal("Hello, I’m testing!", result.Bio);
        }
    }
}
