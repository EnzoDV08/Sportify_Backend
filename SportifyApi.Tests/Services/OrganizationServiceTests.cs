using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Models;
using SportifyApi.Services;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class OrganizationServiceTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task CreateAsync_CreatesOrganizationSuccessfully()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationService(context);

            var dto = new OrganizationDto
            {
                Name = "Test Org",
                Email = "org@test.com",
                Website = "https://org.com",
                ContactPerson = "Admin"
            };

            var result = await service.CreateAsync(dto, "password123");

            Assert.NotNull(result);
            Assert.NotEqual(0, result.OrganizationId);

            var saved = await context.Organizations.FindAsync(result.OrganizationId);
            Assert.NotNull(saved);
            Assert.Equal("Test Org", saved.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrg_WhenExists()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationService(context);

            var created = await service.CreateAsync(new OrganizationDto
            {
                Name = "Find Me",
                Email = "find@org.com"
            }, "password");

            var result = await service.GetByIdAsync(created.OrganizationId);

            Assert.NotNull(result);
            Assert.Equal("Find Me", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllOrganizations()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationService(context);

            await service.CreateAsync(new OrganizationDto { Name = "Org1", Email = "org1@test.com" }, "123");
            await service.CreateAsync(new OrganizationDto { Name = "Org2", Email = "org2@test.com" }, "123");

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesFieldsCorrectly()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationService(context);

            var created = await service.CreateAsync(new OrganizationDto
            {
                Name = "Old Name",
                Email = "old@org.com"
            }, "oldpass");

            var updateDto = new UpdateOrganizationDto
            {
                Name = "New Name",
                Email = "new@org.com",
                Website = "https://new.org",
                ContactPerson = "New Contact"
            };

            var success = await service.UpdateAsync(created.OrganizationId, updateDto);

            Assert.True(success);
            var updated = await context.Organizations.FindAsync(created.OrganizationId);
            Assert.Equal("New Name", updated!.Name);
            Assert.Equal("https://new.org", updated.Website);
        }

        [Fact]
        public async Task DeleteAsync_RemovesOrganization()
        {
            var context = await GetDbContextAsync();
            var service = new OrganizationService(context);

            var created = await service.CreateAsync(new OrganizationDto
            {
                Name = "To Delete",
                Email = "delete@org.com"
            }, "deletepass");

            var success = await service.DeleteAsync(created.OrganizationId);

            Assert.True(success);
            var deleted = await context.Organizations.FindAsync(created.OrganizationId);
            Assert.Null(deleted);
        }
    }
}
