using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _service;

        public AchievementsController(IAchievementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var achievements = await _service.GetAllAchievementsAsync();
            return Ok(achievements);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AchievementDto dto)
        {
            var result = await _service.CreateAchievementAsync(dto);
            if (!result) return BadRequest("Failed to create achievement.");
            return Ok("Achievement created.");
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignToUser(UserAchievementDto dto)
        {
            var result = await _service.AssignToUserAsync(dto);
            if (!result) return BadRequest("Assignment failed.");
            return Ok("Achievement assigned to user.");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAchievements(int userId)
        {
            var achievements = await _service.GetUserAchievementsAsync(userId);
            return Ok(achievements);
        }
    }
}
