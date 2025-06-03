using Microsoft.AspNetCore.Mvc;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAchievementsController : ControllerBase
    {
        private readonly IUserAchievementService _userAchievementService;

        public UserAchievementsController(IUserAchievementService userAchievementService)
        {
            _userAchievementService = userAchievementService;
        }

        // ✅ Assign an achievement to a user (Admin action)
        [HttpPost("assign")]
        public async Task<ActionResult<UserAchievement>> AssignAchievement([FromBody] AssignAchievementDto dto)
        {
            try
            {
                var result = await _userAchievementService.AssignAchievementAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ View all achievements for a specific user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserAchievement>>> GetUserAchievements(int userId)
        {
            var achievements = await _userAchievementService.GetAchievementsByUserAsync(userId);
            return Ok(achievements);
        }

        // ✅ Trigger auto-achievements (e.g., after user joins event)
        [HttpPost("check-auto/{userId}")]
        public async Task<ActionResult> CheckAutoAchievements(int userId)
        {
            await _userAchievementService.CheckAutoAchievementsAsync(userId);
            return Ok("Auto achievements checked.");
        }
    }
}
