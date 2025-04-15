using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SportifyApi.Controllers;
using SportifyApi.Interfaces;
using SportifyApi.Dtos;
using SportifyApi.Models;
using System.Threading.Tasks;

namespace SportifyApi.Test
{
    public class EventsControllerTests
    {
        [Fact]
        public async Task CreateEvent_Returns_CreatedAtActionResult_With_Event()
        {
            // Arrange
            var mockService = new Mock<IEventService>();
            var testEventDto = new EventDto
            {
                Title = "Mocked Event",
                Date = DateTime.UtcNow,
                Location = "Mock Location",
                Type = "Sport",
                Visibility = "Public",
                Status = "Open"
            };

            var createdEvent = new Event
            {
                EventId = 1,
                Title = "Mocked Event",
                Date = testEventDto.Date,
                Location = testEventDto.Location
            };

            mockService
                .Setup(s => s.CreateEventAsync(testEventDto, 1))
                .ReturnsAsync(createdEvent);

            var controller = new EventsController(mockService.Object);

            // Act
            var result = await controller.CreateEvent(testEventDto, 1);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEvent = Assert.IsType<Event>(createdResult.Value);
            Assert.Equal("Mocked Event", returnedEvent.Title);
        }
    }
}
