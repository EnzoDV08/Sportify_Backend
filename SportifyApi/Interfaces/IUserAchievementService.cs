using SportifyApi.Dtos;
using SportifyApi.Models;

namespace SportifyApi.Interfaces
{
    public interface IUserAchievementService
    {
        Task<UserAchievement> AssignAchievementAsync(AssignAchievementDto dto);
        Task<List<UserAchievement>> GetAchievementsByUserAsync(int userId);
        Task CheckAutoAchievementsAsync(int userId);

        Task<bool> UnassignAchievementAsync(UnassignAchievementDto dto);


    }
}
