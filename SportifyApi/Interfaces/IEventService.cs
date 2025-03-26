using SportifyApi.Dtos;
using SportifyApi.Models;

namespace SportifyApi.Interfaces
{
    public interface IEventService
    {
        Task<Event> CreateEventAsync(EventDto eventDto);
        Task<Event?> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> UpdateEventAsync(int id, EventDto updatedEvent);
        Task<bool> DeleteEventAsync(int id);
    }
}