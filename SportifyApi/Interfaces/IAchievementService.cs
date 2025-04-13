using SportifyApi.DTOs;


namespace SportifyApi.Interfaces
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAchievementsAsync();
        Task<bool> CreateAchievementAsync(AchievementDto dto);
        Task<bool> AssignToUserAsync(UserAchievementDto dto);
        Task<IEnumerable<AchievementDto>> GetUserAchievementsAsync(int userId);
        Task CheckAutoAchievementsAsync(int userId);
    }
}
