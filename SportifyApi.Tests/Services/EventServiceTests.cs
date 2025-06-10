using Xunit;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Models;
using SportifyApi.Services;
using SportifyApi.Dtos;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace SportifyApi.Test.Services
{
    public class EventServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldCreateEventWithInvites()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            var user = new User { UserId = 1, Name = "Test User", Email = "test@example.com", Password = "secure123", UserType = "user" };
            var invitee = new User { UserId = 2, Name = "Invitee", Email = "invitee@example.com" };
            context.Users.AddRange(user, invitee);
            await context.SaveChangesAsync();

            var eventDto = new EventDto
            {
                Title = "Test Event",
                Description = "Test Desc",
                StartDateTime = DateTime.UtcNow.AddHours(1),
                EndDateTime = DateTime.UtcNow.AddHours(2),
                Location = "Online",
                SportType = "Soccer",
                Type = "Match",
                Visibility = "Public",
                Status = "Open",
                RequiredItems = "Ball, Shoes",
                ImageUrl = "http://example.com/image.jpg",
                InvitedUserIds = new List<int> { 2 },
                CreatorUserType = "user"
            };

            var created = await service.CreateEventAsync(eventDto, 1);

            created.Should().NotBeNull();
            created.Title.Should().Be("Test Event");

            var participant = await context.EventParticipants.FirstOrDefaultAsync(p => p.UserId == 2);
            participant.Should().NotBeNull();
            participant.Status.Should().Be("Invited");
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnCorrectEvent()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            var user = new User { UserId = 1, Name = "User1", Email = "user@example.com", Password = "pw", UserType = "user" };
            var createdEvent = new Event
            {
                EventId = 1,
                Title = "Test Event",
                Description = "Test Desc",
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(1),
                Location = "Online",
                SportType = "Soccer",
                Type = "Match",
                Visibility = "Public",
                Status = "Open",
                RequiredItems = "Ball",
                ImageUrl = "http://image.com",
                CreatorUserId = 1
            };

            context.Users.Add(user);
            context.Events.Add(createdEvent);
            await context.SaveChangesAsync();

            var result = await service.GetEventByIdAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Test Event");
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            // Add the creator user
            var user = new User { UserId = 1, Name = "Creator", Email = "creator@example.com", Password = "pw", UserType = "user" };
            context.Users.Add(user);

            context.Events.AddRange(
                new Event { EventId = 1, Title = "Event 1", CreatorUserId = 1 },
                new Event { EventId = 2, Title = "Event 2", CreatorUserId = 1 }
            );
            await context.SaveChangesAsync();

            var results = await service.GetAllEventsAsync();

            results.Should().HaveCount(2);
            results.Should().Contain(e => e.Title == "Event 1");
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateEventDetails()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            var existing = new Event
            {
                EventId = 1,
                Title = "Old Title",
                Description = "Old Description",
                CreatorUserId = 1
            };

            context.Events.Add(existing);
            await context.SaveChangesAsync();

            var updateDto = new EventDto
            {
                Title = "New Title",
                Description = "New Desc",
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(1),
                Location = "Park",
                SportType = "Tennis",
                Type = "Game",
                Visibility = "Private",
                Status = "Closed",
                RequiredItems = "Shoes",
                ImageUrl = "http://newimage.com",
                CreatorUserType = "user"
            };

            var result = await service.UpdateEventAsync(1, updateDto);

            result.Should().NotBeNull();
            result.Title.Should().Be("New Title");
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldDeleteEvent()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            var evnt = new Event { EventId = 1, Title = "To Delete", CreatorUserId = 1 };
            context.Events.Add(evnt);
            await context.SaveChangesAsync();

            var deleted = await service.DeleteEventAsync(1);
            deleted.Should().BeTrue();

            var deletedEvent = await context.Events.FindAsync(1);
            deletedEvent.Should().BeNull();
        }

        [Fact]
        public async Task GetEventsCreatedByUserAsync_ShouldReturnUserEvents()
        {
            var context = GetDbContext();
            var service = new EventService(context);

            // Add both users
            context.Users.AddRange(
                new User { UserId = 1, Name = "User1", Email = "user1@example.com", Password = "pw", UserType = "user" },
                new User { UserId = 2, Name = "User2", Email = "user2@example.com", Password = "pw", UserType = "user" }
            );

            context.Events.AddRange(
                new Event { EventId = 1, Title = "User1 Event", CreatorUserId = 1 },
                new Event { EventId = 2, Title = "User2 Event", CreatorUserId = 2 }
            );
            await context.SaveChangesAsync();

            var user1Events = await service.GetEventsCreatedByUserAsync(1);

            user1Events.Should().HaveCount(1);
            user1Events.First().Title.Should().Be("User1 Event");
        }

    }
}
