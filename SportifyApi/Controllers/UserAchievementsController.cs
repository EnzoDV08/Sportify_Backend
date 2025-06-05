using Microsoft.AspNetCore.Mvc;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UserAchievementsController : ControllerBase
    {
        private readonly IUserAchievementService _userAchievementService;

        public UserAchievementsController(IUserAchievementService userAchievementService)
        {
            _userAchievementService = userAchievementService;
        }

<<<<<<< HEAD
        // ✅ Assign an achievement to a user (Admin action)
[HttpPost("assign")]
public async Task<IActionResult> AssignAchievement([FromBody] AssignAchievementDto dto)
{
    try
    {
        var result = await _userAchievementService.AssignAchievementAsync(dto);
        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message }); // 👈 This is what shows in frontend
    }
}

=======
        [HttpPost("assign")]
        [ProducesResponseType(typeof(UserAchievement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
>>>>>>> Enzo

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<UserAchievement>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserAchievement>>> GetUserAchievements(int userId)
        {
            var achievements = await _userAchievementService.GetAchievementsByUserAsync(userId);
            return Ok(achievements);
        }

        [HttpPost("check-auto/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CheckAutoAchievements(int userId)
        {
            await _userAchievementService.CheckAutoAchievementsAsync(userId);
            return Ok("Auto achievements checked.");
        }

        [HttpDelete("unassign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnassignAchievement([FromBody] UnassignAchievementDto dto)
        {
            var result = await _userAchievementService.UnassignAchievementAsync(dto);
            if (!result) return NotFound("Achievement not found or not assigned.");
            return Ok("Achievement unassigned.");
        }

    }
}
