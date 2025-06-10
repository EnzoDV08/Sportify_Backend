using Xunit;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Models;
using SportifyApi.Services;
using SportifyApi.Interfaces;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace SportifyApi.Tests.Services
{
    public class EventParticipantServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task JoinEventAsync_Should_Add_Participant_When_Not_Exists()
        {
            var context = GetDbContext();
            var mockAchievementService = new Mock<IUserAchievementService>();
            var service = new EventParticipantService(context, mockAchievementService.Object);

            var user = new User { UserId = 1, Name = "User" };
            var evnt = new Event { EventId = 1, Title = "Event", CreatorUserId = 1 };
            context.Users.Add(user);
            context.Events.Add(evnt);
            await context.SaveChangesAsync();

            var result = await service.JoinEventAsync(1, 1);
            Assert.True(result);

            var participant = await context.EventParticipants.FirstOrDefaultAsync();
            Assert.NotNull(participant);
            Assert.Equal("Pending", participant.Status);
        }

        [Fact]
        public async Task JoinEventAsync_Should_ReturnFalse_If_AlreadyJoined()
        {
            var context = GetDbContext();
            var mockAchievementService = new Mock<IUserAchievementService>();
            var service = new EventParticipantService(context, mockAchievementService.Object);

            var participant = new EventParticipant { EventId = 1, UserId = 1, Status = "Pending" };
            context.EventParticipants.Add(participant);
            await context.SaveChangesAsync();

            var result = await service.JoinEventAsync(1, 1);
            Assert.False(result);
        }

        [Fact]
        public async Task ApproveRequestAsync_Should_Approve_If_Valid()
        {
            var context = GetDbContext();
            var mockAchievementService = new Mock<IUserAchievementService>();
            var service = new EventParticipantService(context, mockAchievementService.Object);

            var eventObj = new Event { EventId = 1, CreatorUserId = 10 };
            var user = new User { UserId = 10, Name = "Creator", UserType = "user" };
            var participant = new EventParticipant { EventId = 1, UserId = 2, Status = "Pending" };

            context.Events.Add(eventObj);
            context.Users.Add(user);
            context.EventParticipants.Add(participant);
            await context.SaveChangesAsync();

            var result = await service.ApproveRequestAsync(1, 2, 10);
            Assert.True(result);

            var updated = await context.EventParticipants.FirstOrDefaultAsync();
            Assert.NotNull(updated);
            Assert.Equal("Approved", updated!.Status);
        }

        [Fact]
        public async Task RejectRequestAsync_Should_Reject_If_Valid()
        {
            var context = GetDbContext();
            var mockAchievementService = new Mock<IUserAchievementService>();
            var service = new EventParticipantService(context, mockAchievementService.Object);

            var eventObj = new Event { EventId = 1, CreatorUserId = 10 };
            var user = new User { UserId = 10, Name = "Creator", UserType = "user" };
            var participant = new EventParticipant { EventId = 1, UserId = 2, Status = "Pending" };

            context.Events.Add(eventObj);
            context.Users.Add(user);
            context.EventParticipants.Add(participant);
            await context.SaveChangesAsync();

            var result = await service.RejectRequestAsync(1, 2, 10);
            Assert.True(result);

            var updated = await context.EventParticipants.FirstOrDefaultAsync();
            Assert.NotNull(updated);
            Assert.Equal("Rejected", updated!.Status);
        }

        [Fact]
        public async Task RemoveUserFromEventAsync_Should_Remove_Participant()
        {
            var context = GetDbContext();
            var mockAchievementService = new Mock<IUserAchievementService>();
            var service = new EventParticipantService(context, mockAchievementService.Object);

            var participant = new EventParticipant { EventId = 1, UserId = 1, Status = "Approved" };
            context.EventParticipants.Add(participant);
            await context.SaveChangesAsync();

            var result = await service.RemoveUserFromEventAsync(1, 1);
            Assert.True(result);

            var exists = await context.EventParticipants.AnyAsync();
            Assert.False(exists);
        }
    }
}
