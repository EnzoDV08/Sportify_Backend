using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class FriendService : IFriendService
    {
        private readonly AppDbContext _context;

        public FriendService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendRequest(FriendRequestDto dto)
        {
            var exists = await _context.Friends.AnyAsync(f =>
                (f.UserId == dto.SenderId && f.FriendId == dto.ReceiverId) ||
                (f.UserId == dto.ReceiverId && f.FriendId == dto.SenderId));

            if (exists) throw new Exception("Friend request already exists or you're already friends.");

            var friend = new Friend
            {
                UserId = dto.SenderId,
                FriendId = dto.ReceiverId,
                Status = "pending"
            };

            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptRequest(int id)
        {
            var request = await _context.Friends.FindAsync(id);
            if (request == null || request.Status != "pending") throw new Exception("Invalid request.");

            request.Status = "accepted";

            // Optional: add reverse record if desired (mutual friendship)
            var reverse = new Friend
            {
                UserId = request.FriendId,
                FriendId = request.UserId,
                Status = "accepted"
            };
            _context.Friends.Add(reverse);

            await _context.SaveChangesAsync();
        }

        public async Task RejectRequest(int id)
        {
            var request = await _context.Friends.FindAsync(id);
            if (request == null || request.Status != "pending") throw new Exception("Invalid request.");

            request.Status = "rejected";
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFriend(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null) throw new Exception("Friend not found.");

            _context.Friends.Remove(friend);

            // Also remove reverse friend if it exists
            var reverse = await _context.Friends.FirstOrDefaultAsync(f =>
                f.UserId == friend.FriendId && f.FriendId == friend.UserId);
            if (reverse != null) _context.Friends.Remove(reverse);

            await _context.SaveChangesAsync();
        }

        public async Task<List<FullFriendDto>> GetMyFriends(int userId)
        {
            return await _context.Friends
                .Where(f => f.UserId == userId && f.Status == "accepted")
                .Include(f => f.FriendUser)
                .Select(f => new FullFriendDto
                {
                    Id = f.Id,
                    Friend = new SimpleUserDto
                    {
                        UserId = f.FriendUser.UserId,
                        Name = f.FriendUser.Name,
                        ProfilePicture = f.FriendUser.ImageUrl,
                        Bio = f.FriendUser.Profile?.Bio
                    },
                    Status = f.Status
                })
                .ToListAsync();
        }

        public async Task<List<FullFriendDto>> GetRequests(int userId)
        {
            return await _context.Friends
                .Where(f => f.FriendId == userId && f.Status == "pending")
                .Include(f => f.User)
                .Select(f => new FullFriendDto
                {
                    Id = f.Id,
                    Friend = new SimpleUserDto
                    {
                        UserId = f.User.UserId,
                        Name = f.User.Name,
                        ProfilePicture = f.User.ImageUrl,
                        Bio = f.User.Profile?.Bio
                    },
                    Status = f.Status
                })
                .ToListAsync();
        }

        public async Task<List<FullFriendDto>> SearchUsers(string query, int userId)
        {
            var friendIds = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Select(f => f.FriendId == userId ? f.UserId : f.FriendId)
                .ToListAsync();

            return await _context.Users
                .Where(u => (u.Name.Contains(query) || u.Username.Contains(query)) && u.UserId != userId && !friendIds.Contains(u.UserId))
                .Select(u => new FullFriendDto
                {
                    Id = 0,
                    Friend = new SimpleUserDto
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        ProfilePicture = u.ImageUrl,
                        Bio = u.Profile != null ? u.Profile.Bio : null
                    },
                    Status = "not_friends"
                })
                .ToListAsync();
        }
    }
}
