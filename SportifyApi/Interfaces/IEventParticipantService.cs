using SportifyApi.Models;

namespace SportifyApi.Interfaces
{
    public interface IEventParticipantService
    {
        Task<bool> JoinEventAsync(int eventId, int userId);
        Task<List<EventParticipant>> GetPendingRequestsForCreator(int creatorUserId);
        Task<bool> ApproveRequestAsync(int eventId, int userId, int creatorUserId);
        Task<bool> RejectRequestAsync(int eventId, int userId, int creatorUserId);
    }
}