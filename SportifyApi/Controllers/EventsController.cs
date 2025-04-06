using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // ✅ Create a new event
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(EventDto eventDto, int creatorUserId)
        {
            var createdEvent = await _eventService.CreateEventAsync(eventDto, creatorUserId);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.EventId }, createdEvent);
        }

        // ✅ Get a specific event by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var evnt = await _eventService.GetEventByIdAsync(id);
            if (evnt == null)
                return NotFound("Event not found.");

            return Ok(evnt);
        }

        // ✅ Get all events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        // ✅ Update event
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto updatedEvent)
        {
            var result = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (result == null)
                return NotFound("Event not found.");

            return Ok(result);
        }

        // ✅ Delete event
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var deleted = await _eventService.DeleteEventAsync(id);
            if (!deleted)
                return NotFound("Event not found.");

            return NoContent();
        }

        // ✅ NEW ENDPOINT: Get event with participants and admin
        // GET: api/events/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<EventWithParticipantsDto>> GetEventWithParticipants(int id)
        {
            var evnt = await _eventService.GetEventWithParticipantsAsync(id);
            if (evnt == null)
                return NotFound("Event not found.");

            return Ok(evnt);
        }
    }
}
