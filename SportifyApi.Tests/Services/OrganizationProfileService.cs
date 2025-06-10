using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Models;
using SportifyApi.Services;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class OrganizationProfileServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();

            // Add org for FK constraint
            context.Organizations.Add(new Organization
            {
                OrganizationId = 1,
                Name = "Test Org",
                Email = "test@org.com",
                Password = "pass"
            });
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task CreateProfileAsync_SavesProfile()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationProfileService(context);

            var dto = new OrganizationProfileDto
            {
                OrganizationId = 1,
                LogoUrl = "https://img.com/logo.png",
                Description = "Sports Org",
                ContactNumber = "1234567890",
                Location = "Cape Town"
            };

            var result = await service.CreateProfileAsync(dto);

            Assert.Equal("Cape Town", result.Location);
            var saved = await context.OrganizationProfiles.FindAsync(1);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task GetProfileByIdAsync_ReturnsProfile_WhenExists()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationProfileService(context);

            await service.CreateProfileAsync(new OrganizationProfileDto
            {
                OrganizationId = 1,
                Description = "Find Me",
                Location = "Durban"
            });

            var result = await service.GetProfileByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Durban", result.Location);
        }

        [Fact]
        public async Task UpdateProfileAsync_UpdatesFieldsCorrectly()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationProfileService(context);

            await service.CreateProfileAsync(new OrganizationProfileDto
            {
                OrganizationId = 1,
                Description = "Old Desc"
            });

            var updated = new OrganizationProfileDto
            {
                LogoUrl = "https://updated.com/logo.png",
                Description = "New Desc",
                ContactNumber = "000123",
                Location = "Johannesburg"
            };

            var result = await service.UpdateProfileAsync(1, updated);

            Assert.True(result);
            var saved = await context.OrganizationProfiles.FindAsync(1);
            Assert.Equal("New Desc", saved!.Description);
        }

        [Fact]
        public async Task DeleteProfileAsync_RemovesProfile()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationProfileService(context);

            await service.CreateProfileAsync(new OrganizationProfileDto
            {
                OrganizationId = 1,
                Location = "DeleteMe"
            });

            var deleted = await service.DeleteProfileAsync(1);
            var result = await context.OrganizationProfiles.FindAsync(1);

            Assert.True(deleted);
            Assert.Null(result);
        }
    }
}