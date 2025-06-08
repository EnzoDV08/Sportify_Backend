using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;

        // ✅ Single constructor (not duplicated)
        public EventsController(IEventService eventService, AppDbContext context)
        {
            _eventService = eventService;
            _context = context;
        }

        // ✅ Create a new event (admin or user)
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] EventDto eventDto, [FromQuery] int userId)
        {
            var createdEvent = await _eventService.CreateEventAsync(eventDto, userId);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.EventId }, createdEvent);
        }

        // ✅ Get event by ID with creator and participants
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(int id)

        {
            var evnt = await _eventService.GetEventByIdAsync(id);
            if (evnt == null)
                return NotFound("Event not found.");

            return Ok(evnt);
        }

        // ✅ Get all events with creators and participants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()

        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        // ✅ Update an event
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto updatedEvent)
        {
            var result = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (result == null)
                return NotFound("Event not found.");

            return Ok(result);
        }

        // ✅ Delete an event
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var deleted = await _eventService.DeleteEventAsync(id);
            if (!deleted)
                return NotFound("Event not found.");

            return NoContent();
        }

        // ✅ Get all events created by specific user (admin or user)
        [HttpGet("created-by/{userId}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCreator(int userId)
        {
            var events = await _eventService.GetEventsCreatedByUserAsync(userId);
            return Ok(events);
        }

        // ✅ Get participants of a specific event
        [HttpGet("{eventId}/participants")]
        public async Task<IActionResult> GetEventParticipants(int eventId)
        {
            var participants = await _context.EventParticipants
                .Where(p => p.EventId == eventId)
                .Include(p => p.User)
                .Select(p => new
                {
                    p.UserId,
                    Name = p.User.Name,
                    Email = p.User.Email,
                    Status = p.Status
                })
                .ToListAsync();

            return Ok(participants);
        }
    }
}
