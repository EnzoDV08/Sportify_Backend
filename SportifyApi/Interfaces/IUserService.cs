using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(UserDto userDto, string password); // 👈 ADD password here
        Task<bool> UpdateUserAsync(int id, UserDto updatedUser);
        Task<bool> DeleteUserAsync(int id);
    }
}
