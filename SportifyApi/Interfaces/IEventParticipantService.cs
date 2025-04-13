using SportifyApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportifyApi.Interfaces
{
    public interface IEventParticipantService
    {
        Task<bool> JoinEventAsync(int eventId, int userId);
        Task<List<EventParticipant>> GetPendingRequestsAsync(int userId); 
        Task<bool> ApproveRequestAsync(int eventId, int userId, int approverUserId);
        Task<bool> RejectRequestAsync(int eventId, int userId, int approverUserId);
    }
}
