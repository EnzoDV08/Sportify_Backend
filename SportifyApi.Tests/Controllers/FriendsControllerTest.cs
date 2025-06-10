using Microsoft.AspNetCore.Mvc;
using Moq;
using SportifyApi.Controllers;
using SportifyApi.DTOs;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportifyApi.Test.Controllers
{
    public class FriendsControllerTests
    {
        private readonly Mock<IFriendService> _mockService;
        private readonly FriendsController _controller;

        public FriendsControllerTests()
        {
            _mockService = new Mock<IFriendService>();
            _controller = new FriendsController(_mockService.Object);
        }

        [Fact]
        public async Task Send_ReturnsOk()
        {
            var dto = new FriendRequestDto { SenderId = 1, ReceiverId = 2 };

            var result = await _controller.Send(dto);

            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.SendRequestAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Accept_ReturnsOk()
        {
            var result = await _controller.Accept(5);

            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.AcceptRequestAsync(5), Times.Once);
        }

        [Fact]
        public async Task Reject_ReturnsOk()
        {
            var result = await _controller.Reject(8);

            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.RejectRequestAsync(8), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
        {
            var result = await _controller.Delete(3);

            Assert.IsType<OkResult>(result);
            _mockService.Verify(s => s.DeleteFriendAsync(3), Times.Once);
        }

[Fact]
public async Task GetFriends_ReturnsList()
{
    int userId = 1;

    var mockList = new List<FullFriendDto>
    {
        new FullFriendDto
        {
            Id = 1,
            Status = "accepted",
            User = new SimpleUserDto { UserId = 2, Name = "Test Friend", Email = "test@friend.com" },
            Profile = new ProfileDto { UserId = 2, Bio = "Gamer", TotalPoints = 100 }
        }
    };

    _mockService.Setup(s => s.GetMyFriendsAsync(userId)).ReturnsAsync(mockList);

    var result = await _controller.GetFriends(userId);

    var list = Assert.IsAssignableFrom<List<FullFriendDto>>(result.Value);
    Assert.Single(list);
    Assert.Equal("Test Friend", list[0].User.Name);
}


        [Fact]
        public async Task GetRequests_ReturnsList()
        {
            int userId = 1;
            _mockService.Setup(s => s.GetFriendRequestsAsync(userId)).ReturnsAsync(new List<FullFriendDto>());

            var result = await _controller.GetRequests(userId);

            var list = Assert.IsAssignableFrom<List<FullFriendDto>>(result.Value);
            Assert.Empty(list);
        }

        [Fact]
        public async Task Search_ReturnsList()
        {
            string query = "enzo";
            int userId = 1;
            _mockService.Setup(s => s.SearchUsersAsync(query, userId)).ReturnsAsync(new List<FullFriendDto>());

            var result = await _controller.Search(query, userId);

var list = Assert.IsAssignableFrom<List<FullFriendDto>>(result.Value);
Assert.Empty(list);
        }
    }
}