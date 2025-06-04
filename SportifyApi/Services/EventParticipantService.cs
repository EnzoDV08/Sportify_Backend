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
        private readonly IUserAchievementService _userAchievementService;

        public EventParticipantService(AppDbContext context, IUserAchievementService userAchievementService)
        {
            _context = context;
            _userAchievementService = userAchievementService;
        }

        // ✅ Join an event
        public async Task<bool> JoinEventAsync(int eventId, int userId)
        {
            var exists = await _context.EventParticipants
                .AnyAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (exists) return false;

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

        // ✅ Get pending requests for an admin or event creator
        public async Task<List<EventParticipant>> GetPendingRequestsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return new List<EventParticipant>();

            var query = _context.EventParticipants
                .Include(p => p.User)
                .Include(p => p.Event)
                .Where(p => p.Status == "Pending");

            if (user.UserType != "admin")
            {
                query = query.Where(p => p.Event.CreatorUserId == userId);
            }

            return await query.ToListAsync();
        }

        // ✅ Approve participant and trigger auto-achievement
        public async Task<bool> ApproveRequestAsync(int eventId, int userId, int approverUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null) return false;

            var approver = await _context.Users.FindAsync(approverUserId);
            if (approver == null) return false;

            if (approver.UserType != "admin" && evnt.CreatorUserId != approverUserId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (request == null) return false;

            request.Status = "Approved";
            await _context.SaveChangesAsync();

            // ✅ Check for auto-achievements
            await _userAchievementService.CheckAutoAchievementsAsync(userId);

            return true;
        }

        // ✅ Reject participant
        public async Task<bool> RejectRequestAsync(int eventId, int userId, int approverUserId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null) return false;

            var approver = await _context.Users.FindAsync(approverUserId);
            if (approver == null) return false;

            if (approver.UserType != "admin" && evnt.CreatorUserId != approverUserId)
                return false;

            var request = await _context.EventParticipants
                .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId);

            if (request == null) return false;

            request.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}