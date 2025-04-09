using Microsoft.AspNetCore.Mvc;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventParticipantsController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;

        public EventParticipantsController(IEventParticipantService eventParticipantService)
        {
            _eventParticipantService = eventParticipantService;
        }

        // ✅ User requests to join an event
        [HttpPost("join")]
        public async Task<IActionResult> JoinEvent(int eventId, int userId)
        {
            var result = await _eventParticipantService.JoinEventAsync(eventId, userId);
            if (!result)
                return BadRequest("You have already requested to join this event.");

            return Ok("Join request sent successfully.");
        }

        // ✅ Event creator gets all pending join requests
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests([FromQuery] int creatorUserId)
        {
            var requests = await _eventParticipantService.GetPendingRequestsForCreator(creatorUserId);
            return Ok(requests);
        }

        // ✅ Event creator approves a join request
        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRequest(int eventId, int userId, int creatorUserId)
        {
            var result = await _eventParticipantService.ApproveRequestAsync(eventId, userId, creatorUserId);
            if (!result)
                return BadRequest("You are not authorized to approve this request or it doesn't exist.");

            return Ok("Join request approved.");
        }

        // ✅ Event creator rejects a join request
        [HttpPut("reject")]
        public async Task<IActionResult> RejectRequest(int eventId, int userId, int creatorUserId)
        {
            var result = await _eventParticipantService.RejectRequestAsync(eventId, userId, creatorUserId);
            if (!result)
                return BadRequest("You are not authorized to reject this request or it doesn't exist.");

            return Ok("Join request rejected.");
        }
    }
}
