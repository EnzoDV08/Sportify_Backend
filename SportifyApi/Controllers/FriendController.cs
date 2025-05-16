using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFriend([FromBody] FriendDto dto)
        {
            var success = await _friendService.AddFriendAsync(dto);
            return success ? Ok("✅ Friend added.") : BadRequest("❌ Could not add friend.");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFriend([FromBody] FriendDto dto)
        {
            var success = await _friendService.RemoveFriendAsync(dto);
            return success ? Ok("🗑️ Friend removed.") : NotFound("❌ Friend not found.");
        }

        [HttpGet("{userId}/list")]
        public async Task<IActionResult> GetFriends(int userId)
        {
            var friends = await _friendService.GetFriendIdsAsync(userId);
            return Ok(friends);
        }
    }
}
