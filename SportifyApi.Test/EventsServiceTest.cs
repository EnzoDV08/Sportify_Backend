using Xunit;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Models;
using SportifyApi.Services;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using SportifyApi.Dtos; 

namespace SportifyApi.Tests.Services
{
    public class EventServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
public async Task CreateEventAsync_ShouldAddEvent()
{
    // Arrange
    var context = GetDbContext();
    var service = new EventService(context);

    // Add a test user to satisfy foreign key/user validation
    context.Users.Add(new User
    {
        UserId = 1,
        Name = "Test User",
        Email = "test@example.com",
        Password = "hashed",
        UserType = "user"
    });
    await context.SaveChangesAsync();

    var eventDto = new EventDto
    {
        Title = "Test Event",
        Description = "Unit test description",
        Date = DateTime.UtcNow,
        Location = "Pretoria",
        Type = "Soccer",
        Visibility = "Public",
        Status = "Upcoming"
    };

    int userId = 1;
    await service.CreateEventAsync(eventDto, userId);

    // Assert
    var added = await context.Events.FirstOrDefaultAsync(e => e.Title == "Test Event");
    added.Should().NotBeNull();
    added.Description.Should().Be("Unit test description");
}

    }
}