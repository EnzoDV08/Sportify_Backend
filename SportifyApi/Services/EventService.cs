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
                throw new Exception("User not found.");

            // ✅ Ensure DateTime is marked as UTC
            var startUtc = DateTime.SpecifyKind(eventDto.StartDateTime, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(eventDto.EndDateTime, DateTimeKind.Utc);

            var newEvent = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                StartDateTime = startUtc,
                EndDateTime = endUtc,
                Location = eventDto.Location,
                Type = eventDto.Type,
                Visibility = eventDto.Visibility,
                Status = eventDto.Status,
                RequiredItems = eventDto.RequiredItems,
                ImageUrl = eventDto.ImageUrl,
                CreatorUserId = userId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            // ✅ Invite users (if provided)
            if (eventDto.InvitedUserIds != null && eventDto.InvitedUserIds.Any())
            {
                foreach (var invitedUserId in eventDto.InvitedUserIds)
                {
                    _context.EventParticipants.Add(new EventParticipant
                    {
                        EventId = newEvent.EventId,
                        UserId = invitedUserId,
                        Status = "Pending"
                    });
                }

                await _context.SaveChangesAsync();
            }

            Console.WriteLine($"Event created by {eventDto.CreatorUserType} (User ID: {userId})");

            return newEvent;
        }


public async Task<EventDto?> GetEventByIdAsync(int id)
{
    var e = await _context.Events
        .Include(ev => ev.Creator)
        .Include(ev => ev.Participants)
            .ThenInclude(p => p.User)
        .FirstOrDefaultAsync(ev => ev.EventId == id);

    return e == null ? null : ToEventDto(e);
}


public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
{
    var events = await _context.Events
        .Include(e => e.Creator)
        .Include(e => e.Participants)
            .ThenInclude(p => p.User)
        .ToListAsync();

    return events.Select(ToEventDto);
}


        public async Task<IEnumerable<Event>> GetEventsCreatedByUserAsync(int userId)
        {
            return await _context.Events
                .Where(e => e.CreatorUserId == userId)
                .Include(e => e.Creator)
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public async Task<Event?> UpdateEventAsync(int id, EventDto updatedEvent)
        {
            var existing = await _context.Events.FindAsync(id);
            if (existing == null) return null;

            existing.Title = updatedEvent.Title;
            existing.Description = updatedEvent.Description;
            existing.StartDateTime = updatedEvent.StartDateTime;
            existing.EndDateTime = updatedEvent.EndDateTime;
            existing.Location = updatedEvent.Location;
            existing.Type = updatedEvent.Type;
            existing.Visibility = updatedEvent.Visibility;
            existing.Status = updatedEvent.Status;
            existing.RequiredItems = updatedEvent.RequiredItems;
            existing.ImageUrl = updatedEvent.ImageUrl;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null) return false;

            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();
            return true;
        }
        
            private EventDto ToEventDto(Event e)
    {
        return new EventDto
        {
            EventId = e.EventId,
            Title = e.Title,
            Description = e.Description,
            StartDateTime = e.StartDateTime,
            EndDateTime = e.EndDateTime,
            Location = e.Location,
            Type = e.Type,
            Visibility = e.Visibility,
            Status = e.Status,
            RequiredItems = e.RequiredItems,
            ImageUrl = e.ImageUrl,
            CreatorUserType = e.Creator?.UserType ?? "user", // fallback
            CreatorName = e.Creator?.Name ?? "Unknown",
            InvitedUserIds = _context.EventParticipants
                .Where(p => p.EventId == e.EventId)
                .Select(p => p.UserId)
                .ToList()
        };
    }

    }
}

