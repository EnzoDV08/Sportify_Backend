using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SportifyApi.Controllers;
using SportifyApi.Interfaces;
using SportifyApi.Dtos;
using SportifyApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportifyApi.Tests.Controllers
{
    public class EventsControllerTests
    {
        [Fact]
        public async Task CreateEvent_Returns_CreatedAtAction_With_Event()
        {
            var mockService = new Mock<IEventService>();
            var eventDto = new EventDto { Title = "Test Event", CreatorUserType = "user" };

            var createdEvent = new Event { EventId = 99, Title = "Test Event", CreatorUserId = 1 };

            mockService.Setup(s => s.CreateEventAsync(It.IsAny<EventDto>(), 1))
                       .ReturnsAsync(createdEvent);

            var controller = new EventsController(mockService.Object, null!);

            var result = await controller.CreateEvent(eventDto, 1);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnEvent = Assert.IsType<Event>(actionResult.Value);
            Assert.Equal("Test Event", returnEvent.Title);
        }

        [Fact]
        public async Task CreateEvent_WithInvites_ReturnsEventAndInvokesServiceCorrectly()
        {
            // Arrange
            var mockService = new Mock<IEventService>();

            var eventDto = new EventDto
            {
                Title = "Invite Test Event",
                CreatorUserType = "user",
                InvitedUserIds = new List<int> { 2, 3 }
            };

            var createdEvent = new Event
            {
                EventId = 101,
                Title = "Invite Test Event",
                CreatorUserId = 1
            };

            mockService.Setup(s => s.CreateEventAsync(It.IsAny<EventDto>(), 1))
                    .ReturnsAsync(createdEvent);

            var controller = new EventsController(mockService.Object, null!);

            // Act
            var result = await controller.CreateEvent(eventDto, 1);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnEvent = Assert.IsType<Event>(actionResult.Value);
            Assert.Equal("Invite Test Event", returnEvent.Title);

            // Verify that the service was called with correct invited user IDs
            mockService.Verify(s => s.CreateEventAsync(It.Is<EventDto>(
                dto => dto.InvitedUserIds != null && dto.InvitedUserIds.Count == 2), 1), Times.Once);
        }


        [Fact]
        public async Task GetEventById_ReturnsEvent()
        {
            var mockService = new Mock<IEventService>();
            var expectedEvent = new EventDto { EventId = 1, Title = "Sample Event", CreatorUserId = 1 };

            mockService.Setup(s => s.GetEventByIdAsync(1))
                       .ReturnsAsync(expectedEvent);

            var controller = new EventsController(mockService.Object, null!);

            var result = await controller.GetEventById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnEvent = Assert.IsType<EventDto>(okResult.Value);
            Assert.Equal("Sample Event", returnEvent.Title);
        }

        [Fact]
        public async Task GetAllEvents_ReturnsAllEvents()
        {
            var mockService = new Mock<IEventService>();
            var eventList = new List<EventDto> {
                new EventDto { EventId = 1, Title = "Event 1" },
                new EventDto { EventId = 2, Title = "Event 2" }
            };

            mockService.Setup(s => s.GetAllEventsAsync())
                       .ReturnsAsync(eventList);

            var controller = new EventsController(mockService.Object, null!);
            var result = await controller.GetAllEvents();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvents = Assert.IsType<List<EventDto>>(okResult.Value);
            Assert.Equal(2, returnedEvents.Count);
        }

        [Fact]
        public async Task UpdateEvent_ReturnsUpdatedEvent()
        {
            var mockService = new Mock<IEventService>(); // ✅ Declare mockService
            var inputDto = new EventDto { EventId = 1, Title = "Updated Title" }; // ✅ Correct input
            var updatedModel = new Event { EventId = 1, Title = "Updated Title" }; // ✅ Expected output

            mockService.Setup(s => s.UpdateEventAsync(1, It.IsAny<EventDto>()))
                    .ReturnsAsync(updatedModel);

            var controller = new EventsController(mockService.Object, null!);
            var result = await controller.UpdateEvent(1, inputDto); // ✅ Use inputDto here

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnEvent = Assert.IsType<Event>(okResult.Value);
            Assert.Equal("Updated Title", returnEvent.Title);
        }


        [Fact]
        public async Task DeleteEvent_ReturnsNoContent()
        {
            var mockService = new Mock<IEventService>();

            mockService.Setup(s => s.DeleteEventAsync(1))
                       .ReturnsAsync(true);

            var controller = new EventsController(mockService.Object, null!);
            var result = await controller.DeleteEvent(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetEventsByCreator_ReturnsUserEvents()
        {
            var mockService = new Mock<IEventService>();
            var events = new List<Event> {
                new Event { EventId = 1, Title = "Created Event", CreatorUserId = 1 }
            };

            mockService.Setup(s => s.GetEventsCreatedByUserAsync(1))
                       .ReturnsAsync(events);

            var controller = new EventsController(mockService.Object, null!);
            var result = await controller.GetEventsByCreator(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedEvents = Assert.IsType<List<Event>>(okResult.Value);
            Assert.Single(returnedEvents);
            Assert.Equal("Created Event", returnedEvents[0].Title);
        }
    }
}
