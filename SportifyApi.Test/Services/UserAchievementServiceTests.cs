using Microsoft.EntityFrameworkCore;
using Moq;
using SportifyApi.Data;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using SportifyApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportifyApi.Test.Services
{
    public class UserAchievementServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly UserAchievementService _service;

        public UserAchievementServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserAchievementTestDb")
                .Options;
            _mockContext = new Mock<AppDbContext>(options);
            _service = new UserAchievementService(_mockContext.Object);
        }

        [Fact]
        public async Task AssignAchievementAsync_ShouldAssign_WhenValidAdmin()
        {
            // Arrange
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("AssignAchievementTestDb").Options);

            context.Users.Add(new User { UserId = 2, Name = "Admin" });
            context.Users.Add(new User { UserId = 3, Name = "Player" });
            context.Profiles.Add(new Profile { UserId = 3, TotalPoints = 0 });
            context.Achievements.Add(new Achievement { AchievementId = 1, Title = "Test", Points = 100 });
            await context.SaveChangesAsync();

            var service = new UserAchievementService(context);

            var dto = new AssignAchievementDto
            {
                AwardedByUserId = 2,
                UserId = 3,
                AchievementId = 1
            };

            // Act
            var result = await service.AssignAchievementAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.UserId);
            Assert.Equal(1, result.AchievementId);

            var profile = await context.Profiles.FirstAsync(p => p.UserId == 3);
            Assert.Equal(100, profile.TotalPoints);
        }

        [Fact]
        public async Task GetAchievementsByUserAsync_ShouldReturnAchievements()
        {
            // Arrange
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("GetAchievementsTestDb").Options);

            context.UserAchievements.Add(new UserAchievement
            {
                UserId = 4,
                AchievementId = 1,
                Achievement = new Achievement { Title = "Sample", Points = 50 }
            });
            await context.SaveChangesAsync();

            var service = new UserAchievementService(context);

            // Act
            var achievements = await service.GetAchievementsByUserAsync(4);

            // Assert
            Assert.Single(achievements);
            Assert.Equal("Sample", achievements[0].Achievement.Title);
        }

        [Fact]
        public async Task UnassignAchievementAsync_ShouldRemoveAndDeductPoints()
        {
            // Arrange
            var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("UnassignTestDb").Options);

            context.Achievements.Add(new Achievement { AchievementId = 2, Title = "To Remove", Points = 30 });
            context.UserAchievements.Add(new UserAchievement { UserId = 5, AchievementId = 2 });
            context.Profiles.Add(new Profile { UserId = 5, TotalPoints = 30 });
            await context.SaveChangesAsync();

            var service = new UserAchievementService(context);

            var dto = new UnassignAchievementDto { UserId = 5, AchievementId = 2 };

            // Act
            var result = await service.UnassignAchievementAsync(dto);

            // Assert
            Assert.True(result);
            var profile = await context.Profiles.FirstAsync(p => p.UserId == 5);
            Assert.Equal(0, profile.TotalPoints);
        }
    }
}
