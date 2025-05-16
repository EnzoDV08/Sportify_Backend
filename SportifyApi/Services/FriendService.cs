using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SportifyApi.Services
{
    public class FriendService : IFriendService
    {
        private readonly AppDbContext _context;

        public FriendService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddFriendAsync(FriendDto friendDto)
        {
            if (friendDto.UserId == friendDto.FriendUserId) return false;

            var exists = await _context.Friends.AnyAsync(f =>
                f.UserId == friendDto.UserId && f.FriendUserId == friendDto.FriendUserId);

            if (exists) return false;

            _context.Friends.Add(new Friend
            {
                UserId = friendDto.UserId,
                FriendUserId = friendDto.FriendUserId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFriendAsync(FriendDto friendDto)
        {
            var friend = await _context.Friends.FirstOrDefaultAsync(f =>
                f.UserId == friendDto.UserId && f.FriendUserId == friendDto.FriendUserId);

            if (friend == null) return false;

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<int>> GetFriendIdsAsync(int userId)
        {
            return await _context.Friends
                .Where(f => f.UserId == userId)
                .Select(f => f.FriendUserId)
                .ToListAsync();
        }
    }
}
