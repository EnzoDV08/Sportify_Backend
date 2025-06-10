using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Models;
using SportifyApi.Controllers;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using Xunit;
using SportifyApi.Test.Helpers;

namespace SportifyApi.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UsersController(_mockUserService.Object, null!);
        }

        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            var mockUsers = new List<UserDto>
            {
                new UserDto { UserId = 1, Name = "Alice", Email = "alice@test.com" },
                new UserDto { UserId = 2, Name = "Bob", Email = "bob@test.com" }
            };

            _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(mockUsers);

            var result = await _controller.GetUsers();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
            Assert.Equal(2, ((List<UserDto>)users).Count);
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenExists()
        {
            var user = new UserDto { UserId = 1, Name = "Test User", Email = "test@test.com" };

            _mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetUser(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(1, returnedUser.UserId);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenMissing()
        {
            _mockUserService.Setup(s => s.GetUserByIdAsync(10)).ReturnsAsync((UserDto?)null);

            var result = await _controller.GetUser(10);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser()
        {
            var newUser = new UserDto { Name = "New", Email = "new@test.com", Password = "123" };
            var createdUser = new UserDto { UserId = 99, Name = "New", Email = "new@test.com" };

            _mockUserService.Setup(s => s.CreateUserAsync(newUser, "123")).ReturnsAsync(createdUser);

            var result = await _controller.CreateUser(newUser);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var user = Assert.IsType<UserDto>(createdResult.Value);
            Assert.Equal(99, user.UserId);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent_WhenSuccessful()
        {
            var dto = new UserDto { Name = "Updated", Email = "upd@test.com" };
            _mockUserService.Setup(s => s.UpdateUserAsync(1, dto)).ReturnsAsync(true);

            var result = await _controller.UpdateUser(1, dto);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent_WhenDeleted()
        {
            _mockUserService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteUser(1);
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task ToggleTwoFactor_EnablesAndDisables2FA()
        {
            var user = new User { UserId = 1, IsTwoFactorEnabled = false };

            var context = TestHelpers.CreateInMemoryDbContext("Toggle2FA");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(_mockUserService.Object, context);

            var result = await controller.ToggleTwoFactor(1);
            var okResult = Assert.IsType<OkObjectResult>(result);

            var json = JsonSerializer.Serialize(okResult.Value);
            var parsed = JsonSerializer.Deserialize<Dictionary<string, bool>>(json);

            Assert.True(parsed["isTwoFactorEnabled"]);
        }
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            var context = TestHelpers.CreateInMemoryDbContext("LoginValid");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("testpass");

            context.Users.Add(new User
            {
                UserId = 1,
                Email = "test@example.com",
                Name = "Test",
                Password = hashedPassword,
                IsTwoFactorEnabled = true,
                UserType = "user"
            });

            await context.SaveChangesAsync();

            var controller = new UsersController(_mockUserService.Object, context);

            var loginDto = new LoginRequestDto { Email = "test@example.com", Password = "testpass" };
            var result = await controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            var parsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            Assert.Equal(1, parsed["UserId"].GetInt32());
            Assert.Equal("Test", parsed["Name"].GetString());
            Assert.True(parsed["IsTwoFactorEnabled"].GetBoolean());
        }




        [Fact]
        public async Task GetUserByEmail_ReturnsUser()
        {
            var context = TestHelpers.CreateInMemoryDbContext("GetByEmail");
            context.Users.Add(new User { UserId = 1, Email = "test@example.com", IsTwoFactorEnabled = true });
            await context.SaveChangesAsync();

            var controller = new UsersController(_mockUserService.Object, context);

            var result = await controller.GetUserByEmail("test@example.com");
            var okResult = Assert.IsType<OkObjectResult>(result);

            var json = JsonSerializer.Serialize(okResult.Value);
            var parsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            Assert.Equal(1, parsed["UserId"].GetInt32());
            Assert.True(parsed["IsTwoFactorEnabled"].GetBoolean());
        }



    }
}
