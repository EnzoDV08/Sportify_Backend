using Moq;
using Xunit;
using SportifyApi.Services;
using SportifyApi.DTOs;
using SportifyApi.Data;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Models;
using System.Threading.Tasks;

namespace SportifyApi.Tests
{
    public class AdminServiceTests
    {
        private readonly AdminService _adminService;
        private readonly AppDbContext _context;

        public AdminServiceTests()
        {
            // Arrange: Use InMemory Database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminTestDb")
                .Options;

            _context = new AppDbContext(options);
            _adminService = new AdminService(_context);
        }

        [Fact]
        public async Task CreateAdmin_ShouldAddAdmin()
        {
            // Arrange
            var adminDto = new AdminDto
            {
                Name = "Test Admin",
                Email = "admin@test.com",
                UserId = 1
            };

            // Act
            var createdAdmin = await _adminService.CreateAdminAsync(adminDto);

            // Assert
            Assert.NotNull(createdAdmin);
            Assert.Equal("Test Admin", createdAdmin.Name);
            Assert.Equal("admin@test.com", createdAdmin.Email);
            Assert.Equal(1, createdAdmin.UserId);
        }

        [Fact]
        public async Task GetAdminById_ShouldReturnAdmin_WhenExists()
        {
            // Arrange
            var admin = new Admin
            {
                Name = "Existing Admin",
                Email = "existing@test.com",
                UserId = 2
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            // Act
            var foundAdmin = await _adminService.GetAdminByIdAsync(admin.AdminId);

            // Assert
            Assert.NotNull(foundAdmin);
            Assert.Equal("Existing Admin", foundAdmin!.Name);
            Assert.Equal("existing@test.com", foundAdmin.Email);
            Assert.Equal(2, foundAdmin.UserId);
        }

        [Fact]
        public async Task UpdateAdmin_ShouldModifyAdmin_WhenExists()
        {
            // Arrange
            var admin = new Admin
            {
                Name = "Old Name",
                Email = "old@test.com",
                UserId = 3
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            var updatedAdminDto = new AdminDto
            {
                AdminId = admin.AdminId,
                Name = "New Name",
                Email = "new@test.com",
                UserId = 4
            };

            // Act
            var success = await _adminService.UpdateAdminAsync(admin.AdminId, updatedAdminDto);
            var updatedAdmin = await _context.Admins.FindAsync(admin.AdminId);

            // Assert
            Assert.True(success);
            Assert.NotNull(updatedAdmin);
            Assert.Equal("New Name", updatedAdmin!.Name);
            Assert.Equal("new@test.com", updatedAdmin.Email);
            Assert.Equal(4, updatedAdmin.UserId);
        }

        [Fact]
        public async Task DeleteAdmin_ShouldRemoveAdmin_WhenExists()
        {
            // Arrange
            var admin = new Admin
            {
                Name = "To Delete",
                Email = "delete@test.com",
                UserId = 5
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            // Act
            var success = await _adminService.DeleteAdminAsync(admin.AdminId);
            var deletedAdmin = await _context.Admins.FindAsync(admin.AdminId);

            // Assert
            Assert.True(success);
            Assert.Null(deletedAdmin); // Should not exist anymore
        }
    }
}
