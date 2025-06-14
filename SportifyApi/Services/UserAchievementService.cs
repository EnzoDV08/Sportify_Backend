using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        private readonly AppDbContext _context;

        public UserAchievementService(AppDbContext context)
        {
            _context = context;
        }

        // 🔐 Only admin with ID 2 can assign achievements
       public async Task<UserAchievement> AssignAchievementAsync(AssignAchievementDto dto)
{
    try
    {
        var awardingUser = await _context.Users.FindAsync(dto.AwardedByUserId);
        if (awardingUser == null || awardingUser.UserId != 2)
            throw new Exception("Only the admin with ID 2 can assign achievements.");

        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null)
            throw new Exception("Target user not found.");

        var achievement = await _context.Achievements.FindAsync(dto.AchievementId);
        if (achievement == null)
            throw new Exception("Achievement not found.");

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == dto.UserId);
        if (profile == null)
            throw new Exception("Profile for this user not found.");

        var alreadyAssigned = await _context.UserAchievements
            .AnyAsync(ua => ua.UserId == dto.UserId && ua.AchievementId == dto.AchievementId);

        if (alreadyAssigned)
            throw new Exception("User already has this achievement.");

        var userAchievement = new UserAchievement
        {
            AchievementId = dto.AchievementId,
            UserId = dto.UserId,
            EventId = dto.EventId,
            DateAwarded = DateTime.UtcNow
        };

        _context.UserAchievements.Add(userAchievement);

        // 💯 Add points to profile now
        profile.TotalPoints += achievement.Points;

        await _context.SaveChangesAsync();

        return userAchievement;
    }
    catch (Exception ex)
    {
        throw new Exception($"Failed to assign achievement: {ex.Message}");
    }
}


        // 📥 Get all earned achievements for a user
        public async Task<List<UserAchievement>> GetAchievementsByUserAsync(int userId)
        {
            return await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        // 🤖 Auto-assign achievements based on event participation
        public async Task CheckAutoAchievementsAsync(int userId)
        {
            var joinedEvents = await _context.EventParticipants
                .Include(ep => ep.Event)
                .Where(ep => ep.UserId == userId && ep.Status == "Approved")
                .ToListAsync();

            var joinedCount = joinedEvents.Count;

            var allAutoAchievements = await _context.Achievements
                .Where(a => a.IsAutoGenerated)
                .ToListAsync();

            var alreadyEarned = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AchievementId)
                .ToListAsync();

            foreach (var achievement in allAutoAchievements)
            {
                if (alreadyEarned.Contains(achievement.AchievementId))
                    continue;

                bool shouldAward = false;

                // 🎯 Participation-based triggers
                if (achievement.Title == "First Event Joined" && joinedCount >= 1 ||
                    achievement.Title == "Joined 2 Events" && joinedCount >= 2 ||
                    achievement.Title == "Joined 3 Events" && joinedCount >= 3 ||
                    achievement.Title == "Joined 5 Events" && joinedCount >= 5 ||
                    achievement.Title == "10 Events Joined" && joinedCount >= 10 ||
                    achievement.Title == "100 Events Joined" && joinedCount >= 100)
                {
                    shouldAward = true;
                }
                // 🏅 Match by sport type
                else if (!string.IsNullOrEmpty(achievement.SportType))
                {
                    var hasMatchingSport = joinedEvents.Any(ep =>
                        ep.Event?.SportType?.ToLower() == achievement.SportType.ToLower());

                    if (hasMatchingSport)
                    {
                        shouldAward = true;
                    }
                }

                if (shouldAward)
                {
                    _context.UserAchievements.Add(new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = achievement.AchievementId,
                        EventId = null,
                        DateAwarded = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
            await UpdateUserTotalPoints(userId); // 🔁 Sync after auto-assign
        }

        // 🔄 Sync total points based on actual earned achievements
        private async Task UpdateUserTotalPoints(int userId)
        {
            var totalPoints = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Achievement)
                .SumAsync(ua => ua.Achievement.Points);

            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile != null)
            {
                profile.TotalPoints = totalPoints;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UnassignAchievementAsync(UnassignAchievementDto dto)
{
    var record = await _context.UserAchievements
        .FirstOrDefaultAsync(ua => ua.UserId == dto.UserId && ua.AchievementId == dto.AchievementId);

    if (record == null)
        return false;

    _context.UserAchievements.Remove(record);
    await _context.SaveChangesAsync();

    var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == dto.UserId);
    var achievement = await _context.Achievements.FirstOrDefaultAsync(a => a.AchievementId == dto.AchievementId);
    if (profile != null && achievement != null)
    {
        profile.TotalPoints -= achievement.Points;
        if (profile.TotalPoints < 0) profile.TotalPoints = 0;
        await _context.SaveChangesAsync();
    }

    return true;
}

    }
}



