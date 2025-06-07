using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IFriendService
    {
        Task SendRequestAsync(FriendRequestDto dto);
        Task AcceptRequestAsync(int id);
        Task RejectRequestAsync(int id);
        Task DeleteFriendAsync(int id);
        Task<List<FullFriendDto>> GetMyFriendsAsync(int userId);
        Task<List<FullFriendDto>> GetFriendRequestsAsync(int userId);
        Task<List<FullFriendDto>> SearchUsersAsync(string query, int currentUserId);
    }
}
