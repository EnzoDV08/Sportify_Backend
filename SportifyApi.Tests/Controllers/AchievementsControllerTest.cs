using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Controllers;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using Xunit;

namespace SportifyApi.Test.Controllers
{
    public class AchievementsControllerTests
    {
        private readonly Mock<IAchievementService> _achievementServiceMock;
        private readonly AchievementsController _controller;

        public AchievementsControllerTests()
        {
            _achievementServiceMock = new Mock<IAchievementService>();
            _controller = new AchievementsController(_achievementServiceMock.Object);
        }

        [Fact]
        public async Task CreateAchievement_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var dto = new AchievementDto
            {
                AchievementId = 1,
                Title = "MVP",
                Description = "Most Valuable Player",
                SportType = "Football",
                IconUrl = "",
                IsAutoGenerated = false,
                Points = 10
            };

            _achievementServiceMock.Setup(s => s.CreateAchievementAsync(dto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateAchievement(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Achievement created successfully.", okResult.Value);
        }

        [Fact]
        public async Task CreateAchievement_ReturnsBadRequest_WhenFailed()
        {
            var dto = new AchievementDto();
            _achievementServiceMock.Setup(s => s.CreateAchievementAsync(dto))
                .ReturnsAsync(false);

            var result = await _controller.CreateAchievement(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to create achievement.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllAchievements_ReturnsList()
        {
            var achievements = new List<AchievementDto>
            {
                new AchievementDto { AchievementId = 1, Title = "Top Scorer", Description = "Scored the most", SportType = "Basketball", Points = 15 },
                new AchievementDto { AchievementId = 2, Title = "Champion", Description = "Won the tournament", SportType = "Tennis", Points = 20 }
            };
            _achievementServiceMock.Setup(s => s.GetAllAchievementsAsync())
                .ReturnsAsync(achievements);


            var result = await _controller.GetAllAchievements();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<List<AchievementDto>>(okResult.Value);
            Assert.Equal(2, returned.Count);
        }
    }
}