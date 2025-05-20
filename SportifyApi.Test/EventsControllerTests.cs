using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SportifyApi.Controllers;
using SportifyApi.Interfaces;
using SportifyApi.Dtos;
using SportifyApi.Models;
using System;
using System.Threading.Tasks;

namespace SportifyApi.Test
{
    public class EventsControllerTests
    {
        [Fact]
        public async Task CreateEvent_Returns_CreatedAtAction_With_Event()
        {
            var mockService = new Mock<IEventService>();
            var testDto = new EventDto
            {
                Title = "Test Event",
                Date = DateTime.UtcNow,
                Location = "Test Location",
                Type = "Sport",
                Visibility = "Public",
                Status = "Open"
            };

            var testEvent = new Event
            {
                EventId = 1,
                Title = testDto.Title,
                Date = testDto.Date,
                Location = testDto.Location,
                Type = testDto.Type,
                Visibility = testDto.Visibility,
                Status = testDto.Status,
                CreatorUserId = 1
            };

            mockService
                .Setup(service => service.CreateEventAsync(testDto, 1))
                .ReturnsAsync(testEvent);

            var controller = new EventsController(mockService.Object);
            var result = await controller.CreateEvent(testDto, 1);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedEvent = Assert.IsType<Event>(createdResult.Value);
            Assert.Equal("Test Event", returnedEvent.Title);
        }

        [Fact]
        public async Task UpdateEvent_Returns_Ok_With_UpdatedEvent()
        {
            var mockService = new Mock<IEventService>();
            var updatedDto = new EventDto
            {
                Title = "Updated Title",
                Date = DateTime.UtcNow,
                Location = "Updated Location",
                Type = "Sport",
                Visibility = "Public",
                Status = "Open"
            };

            var updatedEvent = new Event
            {
                EventId = 1,
                Title = "Updated Title"
            };

            mockService
                .Setup(service => service.UpdateEventAsync(1, updatedDto))
                .ReturnsAsync(updatedEvent);

            var controller = new EventsController(mockService.Object);
            var result = await controller.UpdateEvent(1, updatedDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEvent = Assert.IsType<Event>(okResult.Value);
            Assert.Equal("Updated Title", returnedEvent.Title);
        }

        [Fact]
        public async Task DeleteEvent_Returns_NoContent_OnSuccess()
        {
            var mockService = new Mock<IEventService>();
            mockService
                .Setup(service => service.DeleteEventAsync(1))
                .ReturnsAsync(true);

            var controller = new EventsController(mockService.Object);
            var result = await controller.DeleteEvent(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}