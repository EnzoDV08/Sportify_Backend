using Microsoft.AspNetCore.Mvc;
using SportifyApi.Dtos;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAchievement([FromBody] AchievementDto dto)
        {
            var result = await _achievementService.CreateAchievementAsync(dto);
            if (result)
                return Ok("Achievement created successfully.");
            return BadRequest("Failed to create achievement.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAchievements()
        {
            var achievements = await _achievementService.GetAllAchievementsAsync();
            return Ok(achievements);
        }

        
    }
}
