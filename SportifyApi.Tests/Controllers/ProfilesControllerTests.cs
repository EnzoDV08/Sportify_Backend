using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Controllers;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportifyApi.Test.Controllers
{
    public class ProfilesControllerTests
    {
        private readonly Mock<IProfileService> _mockService;
        private readonly ProfilesController _controller;

        public ProfilesControllerTests()
        {
            _mockService = new Mock<IProfileService>();
            _controller = new ProfilesController(_mockService.Object);
        }

        [Fact]
        public async Task GetProfiles_ReturnsAllProfiles()
        {
            // Arrange
            var profiles = new List<ProfileDto> { new ProfileDto { UserId = 1 } };
            _mockService.Setup(s => s.GetAllProfilesAsync()).ReturnsAsync(profiles);

            // Act
            var result = await _controller.GetProfiles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ProfileDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetProfile_ExistingId_ReturnsProfile()
        {
            var profile = new ProfileDto { UserId = 1 };
            _mockService.Setup(s => s.GetProfileByIdAsync(1)).ReturnsAsync(profile);

            var result = await _controller.GetProfile(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProfileDto>(okResult.Value);
            Assert.Equal(1, returnValue.UserId);
        }

        [Fact]
        public async Task GetProfile_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetProfileByIdAsync(99)).ReturnsAsync((ProfileDto?)null);

            var result = await _controller.GetProfile(99);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProfile_ReturnsCreatedAtAction()
        {
            var profile = new ProfileDto { UserId = 1 };
            _mockService.Setup(s => s.CreateProfileAsync(It.IsAny<ProfileDto>())).ReturnsAsync(profile);

            var result = await _controller.CreateProfile(profile);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<ProfileDto>(createdResult.Value);
            Assert.Equal(1, returnValue.UserId);
        }

        [Fact]
        public async Task UpdateProfile_ExistingId_ReturnsNoContent()
        {
            _mockService.Setup(s => s.UpdateProfileAsync(1, It.IsAny<ProfileUpdateDto>())).ReturnsAsync(true);

            var result = await _controller.UpdateProfile(1, new ProfileUpdateDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProfile_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.UpdateProfileAsync(99, It.IsAny<ProfileUpdateDto>())).ReturnsAsync(false);

            var result = await _controller.UpdateProfile(99, new ProfileUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProfile_ExistingId_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteProfileAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteProfile(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProfile_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteProfileAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteProfile(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}