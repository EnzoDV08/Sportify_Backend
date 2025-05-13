using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

    public async Task<Event> CreateEventAsync(EventDto eventDto, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found for event creation.");

        var newEvent = new Event
        {
            Title = eventDto.Title,
            Date = eventDto.Date.ToUniversalTime(),
            Location = eventDto.Location,
            Type = eventDto.Type,
            Visibility = eventDto.Visibility,
            Status = eventDto.Status,
            CreatorUserId = userId
        };

        if (user.UserType == "admin")
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
            if (admin == null)
                throw new Exception("Admin not found in Admins table.");

            newEvent.AdminId = admin.AdminId;
        }
        else if (user.UserType == "user")
        {
            newEvent.CreatorUserId = userId;
        }
        else
        {
            throw new Exception("Invalid user type.");
        }

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }


        public async Task<Event?> UpdateEventAsync(int id, EventDto updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
                return null;

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Date = updatedEvent.Date;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Type = updatedEvent.Type;
            existingEvent.Visibility = updatedEvent.Visibility;
            existingEvent.Status = updatedEvent.Status;

            await _context.SaveChangesAsync();
            return existingEvent;
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.FindAsync(id);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null)
                return false;

            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
