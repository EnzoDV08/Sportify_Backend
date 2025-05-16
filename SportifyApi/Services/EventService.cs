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
                Description = eventDto.Description,
                Date = eventDto.Date.ToUniversalTime(),
                Location = eventDto.Location,
                Type = eventDto.Type,
                Visibility = eventDto.Visibility,
                Status = eventDto.Status,
                IsPrivate = eventDto.IsPrivate,
                Latitude = eventDto.Latitude ?? 0,
                Longitude = eventDto.Longitude ?? 0
            };

            if (user.UserType == "admin")
            {
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
                if (admin == null)
                    throw new Exception("Admin not found in Admins table.");

                newEvent.AdminId = admin.AdminId;
            }
            else
            {
                newEvent.CreatorUserId = userId;
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            // Assign invited users
            if (eventDto.InvitedUserIds != null && eventDto.InvitedUserIds.Any())
            {
                foreach (var invitedUserId in eventDto.InvitedUserIds)
                {
                    var participation = new EventParticipant
                    {
                        EventId = newEvent.EventId,
                        UserId = invitedUserId
                    };
                    _context.EventParticipants.Add(participation);
                }
                await _context.SaveChangesAsync();
            }

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
            existingEvent.IsPrivate = updatedEvent.IsPrivate;
            existingEvent.InvitedUserIds = updatedEvent.InvitedUserIds;
            existingEvent.Latitude = updatedEvent.Latitude;
            existingEvent.Longitude = updatedEvent.Longitude;

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

        public async Task<IEnumerable<Event>> GetEventsVisibleToUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new List<Event>();

            var userFriends = await _context.Friends
                .Where(f => f.UserId == userId)
                .Select(f => f.FriendUserId)
                .ToListAsync();

            return await _context.Events
                .Where(e =>
                    !e.IsPrivate ||
                    (e.IsPrivate &&
                        (e.InvitedUserIds != null && e.InvitedUserIds.Contains(userId)) ||
                        (e.CreatorUserId != null && userFriends.Contains(e.CreatorUserId.Value))
                    )
                )
                .ToListAsync();
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
