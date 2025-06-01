using SportifyApi.Dtos;
using SportifyApi.Models;

namespace SportifyApi.Interfaces
{
    public interface IEventService
    {
        // Creates an event and assigns the creator (user or admin)
        Task<Event> CreateEventAsync(EventDto eventDto, int userId);

        // Gets one event with creator and participants
        Task<EventDto?> GetEventByIdAsync(int id);

        // Gets all events (includes participants and creator info)
       Task<IEnumerable<EventDto>> GetAllEventsAsync();

        // Gets all events created by this user/admin
        Task<IEnumerable<Event>> GetEventsCreatedByUserAsync(int userId);

        // Updates an existing event
        Task<Event?> UpdateEventAsync(int id, EventDto updatedEvent);

        // Deletes an event
        Task<bool> DeleteEventAsync(int id);
    }
}
