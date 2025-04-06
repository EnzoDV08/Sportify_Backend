using Microsoft.AspNetCore.Mvc;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // POST: api/EventParticipants/JoinEvent
        [HttpPost("JoinEvent")]
        public async Task<IActionResult> JoinEvent(int eventId, int userId)
        {
            var success = await _eventParticipantService.JoinEventAsync(eventId, userId);
            if (success)
                return Ok("Successfully joined the event.");
            return BadRequest("Failed to join the event.");
        }

        // GET: api/EventParticipants/PendingRequests/{adminId}
        [HttpGet("PendingRequests/{adminId}")]
        public async Task<IActionResult> GetPendingRequests(int adminId)
        {
            var requests = await _eventParticipantService.GetPendingRequestsForAdmin(adminId);
            if (requests != null)
                return Ok(requests);
            return NotFound("No pending requests found.");
        }

        // POST: api/EventParticipants/ApproveRequest
        [HttpPost("ApproveRequest")]
        public async Task<IActionResult> ApproveRequest(int eventId, int userId, int adminId)
        {
            var success = await _eventParticipantService.ApproveRequestAsync(eventId, userId, adminId);
            if (success)
                return Ok("Request approved.");
            return BadRequest("Failed to approve the request.");
        }

        // POST: api/EventParticipants/RejectRequest
        [HttpPost("RejectRequest")]
        public async Task<IActionResult> RejectRequest(int eventId, int userId, int adminId)
        {
            var success = await _eventParticipantService.RejectRequestAsync(eventId, userId, adminId);
            if (success)
                return Ok("Request rejected.");
            return BadRequest("Failed to reject the request.");
        }
    }
}
