using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class EventParticipantService : IEventParticipantService
    {
        private readonly AppDbContext _context;

        public EventParticipantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> JoinEventAsync(int eventId, int userId)
        {
            var existing = await _context.EventParticipants
                .FirstOrDefaultAsync(e => e.EventId == eventId && e.UserId == userId);

            if (existing != null)
                return false; // Already requested to join

            var participant = new EventParticipant
            {
                EventId = eventId,
                UserId = userId,
                Status = "Pending"
            };

            _context.EventParticipants.Add(participant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EventParticipant>> GetPendingRequestsForCreator(int creatorUserId)
        {
            return await _context.EventParticipants
                .Where(p => p.Status == "Pending" && p.Event != null && p.Event.CreatorUserId == creatorUserId)
                .Include(p => p.User)
                .Include(p => p.Event)
                .ToListAsync();
        }

        public async Task<bool> ApproveRequestAsync(int eventId, int userId, int creatorUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null || evnt.CreatorUserId != creatorUserId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            if (request == null)
                return false;

            request.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequestAsync(int eventId, int userId, int creatorUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null || evnt.CreatorUserId != creatorUserId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId);

            if (request == null)
                return false;

            request.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
