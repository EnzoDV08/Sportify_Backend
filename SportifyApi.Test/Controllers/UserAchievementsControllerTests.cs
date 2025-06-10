using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Controllers;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportifyApi.Test.Controllers
{
    public class UserAchievementsControllerTests
    {
        private readonly Mock<IUserAchievementService> _mockService;
        private readonly UserAchievementsController _controller;

        public UserAchievementsControllerTests()
        {
            _mockService = new Mock<IUserAchievementService>();
            _controller = new UserAchievementsController(_mockService.Object);
        }

        [Fact]
        public async Task AssignAchievement_ReturnsOk()
        {
            var dto = new AssignAchievementDto { UserId = 1, AchievementId = 1 };
            var ua = new UserAchievement { UserId = 1, AchievementId = 1 };

            _mockService.Setup(s => s.AssignAchievementAsync(dto)).ReturnsAsync(ua);

            var result = await _controller.AssignAchievement(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(ua, okResult.Value);
        }

        [Fact]
        public async Task GetUserAchievements_ReturnsList()
        {
            int userId = 1;
            var mockAchievements = new List<UserAchievement> { new UserAchievement { UserId = 1, AchievementId = 1 } };

            _mockService.Setup(s => s.GetAchievementsByUserAsync(userId)).ReturnsAsync(mockAchievements);

            var result = await _controller.GetUserAchievements(userId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var achievements = Assert.IsType<List<UserAchievement>>(okResult.Value);
            Assert.Single(achievements);
        }

        [Fact]
        public async Task CheckAutoAchievements_ReturnsOk()
        {
            int userId = 1;

            var result = await _controller.CheckAutoAchievements(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Auto achievements checked.", okResult.Value);
        }

        [Fact]
        public async Task UnassignAchievement_ReturnsOk()
        {
            var dto = new UnassignAchievementDto { UserId = 1, AchievementId = 1 };
            _mockService.Setup(s => s.UnassignAchievementAsync(dto)).ReturnsAsync(true);

            var result = await _controller.UnassignAchievement(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Achievement unassigned.", okResult.Value);
        }

        [Fact]
        public async Task UnassignAchievement_ReturnsNotFound()
        {
            var dto = new UnassignAchievementDto { UserId = 1, AchievementId = 999 };
            _mockService.Setup(s => s.UnassignAchievementAsync(dto)).ReturnsAsync(false);

            var result = await _controller.UnassignAchievement(dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}