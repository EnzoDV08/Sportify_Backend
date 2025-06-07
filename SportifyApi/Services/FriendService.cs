using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Dtos;
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

        public async Task SendRequestAsync(FriendRequestDto dto)
        {
            var exists = await _context.Friends.AnyAsync(f =>
                (f.UserId == dto.SenderId && f.FriendId == dto.ReceiverId) ||
                (f.UserId == dto.ReceiverId && f.FriendId == dto.SenderId));

            if (exists)
                throw new Exception("Request already exists");

            var friend = new Friend
            {
                UserId = dto.SenderId,
                FriendId = dto.ReceiverId,
                Status = "pending"
            };

            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptRequestAsync(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null) throw new Exception("Friend request not found");

            friend.Status = "accepted";
            await _context.SaveChangesAsync();
        }

        public async Task RejectRequestAsync(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null) throw new Exception("Friend request not found");

            friend.Status = "rejected";
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFriendAsync(int id)
        {
            var friend = await _context.Friends.FindAsync(id);
            if (friend == null) throw new Exception("Friend not found");

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FullFriendDto>> GetMyFriendsAsync(int userId)
        {
            var friendEntries = await _context.Friends
                .Where(f => (f.UserId == userId || f.FriendId == userId) && f.Status == "accepted")
                .ToListAsync();

            var result = new List<FullFriendDto>();

            foreach (var f in friendEntries)
            {
                var friendId = f.UserId == userId ? f.FriendId : f.UserId;
                var user = await _context.Users.FindAsync(friendId);
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == friendId);

                if (user != null)
                {
                    result.Add(new FullFriendDto
                    {
                        Id = f.Id,
                        Status = f.Status,
                       User = new SimpleUserDto
{
    UserId = user.UserId,
    Name = user.Name,
    Email = user.Email
},
Profile = new ProfileDto
{
    UserId = profile?.UserId ?? user.UserId,
    ProfilePicture = profile?.ProfilePicture,
    Bio = profile?.Bio,
    FavoriteSports = profile?.FavoriteSports,
    TotalPoints = profile?.TotalPoints ?? 0
},

                    });
                }
            }

            return result;
        }

        public async Task<List<FullFriendDto>> GetFriendRequestsAsync(int userId)
        {
            var friendEntries = await _context.Friends
                .Where(f => f.FriendId == userId && f.Status == "pending")
                .ToListAsync();

            var result = new List<FullFriendDto>();

            foreach (var f in friendEntries)
            {
                var senderId = f.UserId;
                var user = await _context.Users.FindAsync(senderId);
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == senderId);

                if (user != null)
                {
                    result.Add(new FullFriendDto
                    {
                        Id = f.Id,
                        Status = f.Status,
                       User = new SimpleUserDto
{
    UserId = user.UserId,
    Name = user.Name,
    Email = user.Email
},
Profile = new ProfileDto
{
    UserId = profile?.UserId ?? user.UserId,
    ProfilePicture = profile?.ProfilePicture,
    Bio = profile?.Bio,
    FavoriteSports = profile?.FavoriteSports,
    TotalPoints = profile?.TotalPoints ?? 0
}

                    });
                }
            }

            return result;
        }

        public async Task<List<FullFriendDto>> SearchUsersAsync(string query, int currentUserId)
        {
            var users = await _context.Users
                .Where(u => u.Name.ToLower().Contains(query.ToLower()) && u.UserId != currentUserId)
                .ToListAsync();

            var result = new List<FullFriendDto>();

            foreach (var user in users)
            {
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.UserId);

                result.Add(new FullFriendDto
                {
                    Id = 0,
                    Status = "none",
                   User = new SimpleUserDto
{
    UserId = user.UserId,
    Name = user.Name,
    Email = user.Email
},
Profile = new ProfileDto
{
    UserId = profile?.UserId ?? user.UserId,
    ProfilePicture = profile?.ProfilePicture,
    Bio = profile?.Bio,
    FavoriteSports = profile?.FavoriteSports,
    TotalPoints = profile?.TotalPoints ?? 0
}

                });
            }

            return result;
        }
    }
}
