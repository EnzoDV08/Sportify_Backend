using SportifyApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportifyApi.Interfaces
{
    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetAllAchievementsAsync();
        Task<bool> CreateAchievementAsync(AchievementDto dto);
        Task<bool> AssignToUserAsync(UserAchievementDto dto);
        Task<IEnumerable<AchievementDto>> GetUserAchievementsAsync(int userId);

        // ✅ Add this:
        Task CheckAutoAchievementsAsync(int userId);
    }
}
