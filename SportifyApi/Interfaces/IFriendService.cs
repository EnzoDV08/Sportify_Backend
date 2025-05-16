using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IFriendService
    {
        Task<bool> AddFriendAsync(FriendDto friendDto);
        Task<bool> RemoveFriendAsync(FriendDto friendDto);
        Task<List<int>> GetFriendIdsAsync(int userId);
    }
}
