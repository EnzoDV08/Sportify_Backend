using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendsController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] FriendRequestDto dto)
        {
            await _friendService.SendRequestAsync(dto);
            return Ok();
        }

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            await _friendService.AcceptRequestAsync(id);
            return Ok();
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            await _friendService.RejectRequestAsync(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _friendService.DeleteFriendAsync(id);
            return Ok();
        }

        [HttpGet("my-friends/{userId}")]
        public async Task<ActionResult<List<FullFriendDto>>> GetFriends(int userId)
        {
            return await _friendService.GetMyFriendsAsync(userId);
        }

        [HttpGet("requests/{userId}")]
        public async Task<ActionResult<List<FullFriendDto>>> GetRequests(int userId)
        {
            return await _friendService.GetFriendRequestsAsync(userId);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<FullFriendDto>>> Search([FromQuery] string query, [FromQuery] int currentUserId)
        {
            return await _friendService.SearchUsersAsync(query, currentUserId);
        }
    }
}
