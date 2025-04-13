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

        // Get pending requests for UserID and AdminID
        public async Task<List<EventParticipant>> GetPendingRequestsAsync(int userId)
        {
            var isAdmin = await _context.Admins.AnyAsync(a => a.UserId == userId);

            if (isAdmin)
            {
                return await _context.EventParticipants
                    .Where(p => p.Status == "Pending")
                    .Include(p => p.User)
                    .Include(p => p.Event)
                    .ToListAsync();
            }
            else
            {
                return await _context.EventParticipants
                    .Where(p => p.Status == "Pending" && p.Event.CreatorUserId == userId)
                    .Include(p => p.User)
                    .Include(p => p.Event)
                    .ToListAsync();
            }
        }


        // Approve request
        public async Task<bool> ApproveRequestAsync(int eventId, int userId, int approverUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null)
                return false;

            var isAdmin = await _context.Admins.AnyAsync(a => a.UserId == approverUserId);

            if (!isAdmin && evnt.CreatorUserId != approverUserId)
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
        public async Task<bool> RejectRequestAsync(int eventId, int userId, int approverUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null)
                return false;

            var isAdmin = await _context.Admins.AnyAsync(a => a.UserId == approverUserId);

            if (!isAdmin && evnt.CreatorUserId != approverUserId)
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
