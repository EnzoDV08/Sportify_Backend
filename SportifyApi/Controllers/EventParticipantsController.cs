using Microsoft.AspNetCore.Mvc;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventParticipantsController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IUserAchievementService _userAchievementService;

        public EventParticipantsController(
            IEventParticipantService eventParticipantService,
            IUserAchievementService userAchievementService)
        {
            _eventParticipantService = eventParticipantService;
            _userAchievementService = userAchievementService;
        }

        // ✅ POST: api/EventParticipants/JoinEvent
        [HttpPost("JoinEvent")]
        public async Task<IActionResult> JoinEvent(int eventId, int userId)
        {
            var success = await _eventParticipantService.JoinEventAsync(eventId, userId);

            if (success)
            {
                // ✅ Trigger auto achievements after joining
                await _userAchievementService.CheckAutoAchievementsAsync(userId);

                return Ok("Successfully joined the event. Achievement check completed.");
            }

            return BadRequest("Failed to join the event.");
        }

        // ✅ GET: api/EventParticipants/PendingRequests/{userId}
        [HttpGet("PendingRequests/{userId}")]
        public async Task<IActionResult> GetPendingRequests(int userId)
        {
            var requests = await _eventParticipantService.GetPendingRequestsAsync(userId);

            if (requests != null && requests.Any())
                return Ok(requests);

            return NotFound("No pending requests found.");
        }

        // ✅ POST: api/EventParticipants/ApproveRequest
        [HttpPost("ApproveRequest")]
        public async Task<IActionResult> ApproveRequest(int eventId, int userId, int approverUserId)
        {
            var success = await _eventParticipantService.ApproveRequestAsync(eventId, userId, approverUserId);

            if (success)
                return Ok("Request approved.");

            return BadRequest("Failed to approve request.");
        }

        // ✅ POST: api/EventParticipants/RejectRequest
        [HttpPost("RejectRequest")]
        public async Task<IActionResult> RejectRequest(int eventId, int userId, int approverUserId)
        {
            var success = await _eventParticipantService.RejectRequestAsync(eventId, userId, approverUserId);

            if (success)
                return Ok("Request rejected.");

            return BadRequest("Failed to reject request.");
        }
    }
}
