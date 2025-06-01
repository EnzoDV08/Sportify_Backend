using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using SportifyApi.Data;
using SportifyApi.Dtos;
using SportifyApi.Models;
using SportifyApi.Services;

namespace SportifyApi.Tests.Services
{
    public class EventServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public EventServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateEventAsync_ShouldCreateEvent()
        {
            using var context = new AppDbContext(_options);
            context.Users.Add(new User { UserId = 1, Name = "Admin", Email = "admin@mail.com", Password = "123", UserType = "admin" });
            await context.SaveChangesAsync();

            var service = new EventService(context);
            var newEvent = new EventDto
            {
                Title = "Test Event",
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(2),
                Location = "Test Field"
            };

            var result = await service.CreateEventAsync(newEvent, 1);

            Assert.NotNull(result);
            Assert.Equal("Test Event", result.Title);
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnEvent()
        {
            using var context = new AppDbContext(_options);
            var ev = new Event
            {
                Title = "Find Me",
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(1),
                Location = "Stadium",
                CreatorUserId = 1
            };
            context.Users.Add(new User { UserId = 1, Name = "User", Email = "user@mail.com", Password = "123" });
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            var service = new EventService(context);
            var found = await service.GetEventByIdAsync(ev.EventId);

            Assert.NotNull(found);
            Assert.Equal("Find Me", found.Title);
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldRemoveEvent()
        {
            using var context = new AppDbContext(_options);
            var ev = new Event
            {
                Title = "Delete This",
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(1),
                Location = "Gym",
                CreatorUserId = 1
            };
            context.Users.Add(new User { UserId = 1, Name = "Test", Email = "test@mail.com", Password = "123" });
            context.Events.Add(ev);
            await context.SaveChangesAsync();

            var service = new EventService(context);
            var deleted = await service.DeleteEventAsync(ev.EventId);

            Assert.True(deleted);
        }
    }
}
