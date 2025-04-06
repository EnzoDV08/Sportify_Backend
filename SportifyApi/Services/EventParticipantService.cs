using SportifyApi.Data;
using SportifyApi.Models;
using SportifyApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportifyApi.Services
{
    public class EventParticipantService : IEventParticipantService
    {
        private readonly AppDbContext _context;

        public EventParticipantService(AppDbContext context)
        {
            _context = context;
        }

        // Join an event
        public async Task<bool> JoinEventAsync(int eventId, int userId)
        {
            var existingParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (existingParticipant != null)
                return false; // Already a participant

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

        // Get pending requests for admin
        public async Task<List<EventParticipant>> GetPendingRequestsForAdmin(int adminId)
        {
            return await _context.EventParticipants
                .Include(ep => ep.Event)
                .Include(ep => ep.User)
                .Where(ep => ep.Event.AdminId == adminId && ep.Status == "Pending")
                .ToListAsync();
        }

        // Approve request
        public async Task<bool> ApproveRequestAsync(int eventId, int userId, int adminId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null || evnt.AdminId != adminId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (request == null)
                return false;

            request.Status = "Approved";
            await _context.SaveChangesAsync();
            return true;
        }

        // Reject request
        public async Task<bool> RejectRequestAsync(int eventId, int userId, int adminId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null || evnt.AdminId != adminId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (request == null)
                return false;

            request.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
