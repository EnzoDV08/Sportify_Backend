using Microsoft.AspNetCore.Mvc;
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

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(EventDto eventDto)
        {
            var createdEvent = await _eventService.CreateEventAsync(eventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.EventId }, createdEvent);
        }

        // GET: api/Events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(int id)
        {
            var evnt = await _eventService.GetEventByIdAsync(id);
            if (evnt == null)
                return NotFound();

            return Ok(evnt);
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        // PUT api/Events/
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto updatedEvent)
        {
            var result = await _eventService.UpdateEventAsync(id, updatedEvent);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var deleted = await _eventService.DeleteEventAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
