using SportifyApi.Dtos;

namespace SportifyApi.Interfaces
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAchievementsAsync();
        Task<bool> CreateAchievementAsync(AchievementDto dto);

        Task<IEnumerable<AchievementDto>> GetAchievementsBySportAsync(string sport);

    }
}