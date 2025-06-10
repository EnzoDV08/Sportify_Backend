using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Models;
using SportifyApi.Services;
using System.Threading.Tasks;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class ProfileServiceTests
    {
        private AppDbContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("ProfileDb_" + Guid.NewGuid())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateProfileAsync_ShouldAddProfile()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);

            var dto = new ProfileDto { UserId = 1, Bio = "Test Bio" };
            var result = await service.CreateProfileAsync(dto);

            Assert.Equal(1, result.UserId);
            Assert.Equal("Test Bio", result.Bio);
        }

        [Fact]
        public async Task GetAllProfilesAsync_ShouldReturnProfiles()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);
            context.Profiles.Add(new Profile { UserId = 1, Bio = "Bio" });
            await context.SaveChangesAsync();

            var result = await service.GetAllProfilesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetProfileByIdAsync_ShouldReturnCorrectProfile()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);

            context.Users.Add(new User { UserId = 1, Name = "John" });
            context.Profiles.Add(new Profile { UserId = 1, Bio = "Bio" });
            await context.SaveChangesAsync();

            var result = await service.GetProfileByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.UserId);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldModifyFields()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);

            context.Users.Add(new User { UserId = 1, Name = "Old" });
            context.Profiles.Add(new Profile { UserId = 1, Bio = "Old Bio" });
            await context.SaveChangesAsync();

            var update = new ProfileUpdateDto { Bio = "New Bio", Name = "New" };
            var success = await service.UpdateProfileAsync(1, update);
            var updated = await context.Profiles.FindAsync(1);

            Assert.True(success);
            Assert.Equal("New Bio", updated!.Bio);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldReturnFalse_WhenUserOrProfileNotFound()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);

            // No users or profiles added

            var update = new ProfileUpdateDto { Bio = "Should not update" };
            var result = await service.UpdateProfileAsync(999, update); // ID does not exist

            Assert.False(result); // Expect false when user/profile not found
        }

        [Fact]
        public async Task DeleteProfileAsync_ShouldRemoveProfile()
        {
            var context = GetInMemoryDb();
            var service = new ProfileService(context);

            context.Profiles.Add(new Profile { UserId = 1 });
            await context.SaveChangesAsync();

            var success = await service.DeleteProfileAsync(1);
            var profile = await context.Profiles.FindAsync(1);

            Assert.True(success);
            Assert.Null(profile);
        }
    }
}