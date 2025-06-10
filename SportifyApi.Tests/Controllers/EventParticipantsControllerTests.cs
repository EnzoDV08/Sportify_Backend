using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SportifyApi.Controllers;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using SportifyApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SportifyApi.Tests.Controllers
{
    public class EventParticipantsControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);

            // Seed data
            context.Events.Add(new Event { EventId = 1, Title = "Test Event", CreatorUserId = 1 });
            context.Users.Add(new User { UserId = 1, UserType = "user" });

            context.EventParticipants.Add(new EventParticipant
            {
                EventId = 1,
                UserId = 2,
                Status = "Invited"
            });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task JoinEvent_ReturnsOk_WhenSuccess()
        {
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            mockService.Setup(s => s.JoinEventAsync(1, 1)).ReturnsAsync(true);
            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, GetDbContext());

            var result = await controller.JoinEvent(1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully joined the event. Achievement check completed.", okResult.Value);
        }

        [Fact]
        public async Task ApproveRequest_ReturnsOk_WhenApproved()
        {
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            mockService.Setup(s => s.ApproveRequestAsync(1, 2, 3)).ReturnsAsync(true);
            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, GetDbContext());

            var result = await controller.ApproveRequest(1, 2, 3);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Request approved.", okResult.Value);
        }

        [Fact]
        public async Task RejectRequest_ReturnsOk_WhenRejected()
        {
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            mockService.Setup(s => s.RejectRequestAsync(1, 2, 3)).ReturnsAsync(true);
            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, GetDbContext());

            var result = await controller.RejectRequest(1, 2, 3);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Request rejected.", okResult.Value);
        }

        [Fact]
        public async Task RemoveUserFromEvent_ReturnsOk_WhenRemoved()
        {
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            mockService.Setup(s => s.RemoveUserFromEventAsync(1, 2)).ReturnsAsync(true);
            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, GetDbContext());

            var result = await controller.RemoveUserFromEvent(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User removed from event.", okResult.Value);
        }

        [Fact]
        public async Task AcceptInvite_ReturnsOk_WhenSuccessful()
        {
            var context = GetDbContext();
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, context);

            var result = await controller.AcceptInvite(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Invite accepted.", okResult.Value);
        }

        [Fact]
        public async Task RejectInvite_ReturnsOk_WhenSuccessful()
        {
            var context = GetDbContext();
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, context);

            var result = await controller.RejectInvite(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Invite rejected.", okResult.Value);
        }

        [Fact]
        public async Task GetInvitedEvents_ReturnsEvents()
        {
            var context = GetDbContext();
            var mockService = new Mock<IEventParticipantService>();
            var mockAchievements = new Mock<IUserAchievementService>();

            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, context);
            var result = await controller.GetInvitedEvents(2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var events = Assert.IsAssignableFrom<IEnumerable<Event>>(okResult.Value);
            Assert.Single(events);
        }
        [Fact]
        public async Task GetPendingRequests_ReturnsRequestsForCreator()
        {
            var context = GetDbContext();


            context.Users.Add(new User { UserId = 3, Name = "Requester", UserType = "user" });

            context.EventParticipants.Add(new EventParticipant
            {
                EventId = 1,
                UserId = 3,
                Status = "Pending"
            });

            await context.SaveChangesAsync();


            var expectedPendingRequests = context.EventParticipants
                .Include(ep => ep.Event)
                .Where(ep => ep.Status == "Pending" && ep.Event.CreatorUserId == 1)
                .ToList();


            var mockService = new Mock<IEventParticipantService>();
            mockService.Setup(s => s.GetPendingRequestsAsync(1)).ReturnsAsync(expectedPendingRequests);

            var mockAchievements = new Mock<IUserAchievementService>();

            var controller = new EventParticipantsController(mockService.Object, mockAchievements.Object, context);


            var result = await controller.GetPendingRequests(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var requests = Assert.IsAssignableFrom<List<EventParticipant>>(okResult.Value);
            Assert.Single(requests);
            Assert.Equal(3, requests.First().UserId);
        }

    }
}
