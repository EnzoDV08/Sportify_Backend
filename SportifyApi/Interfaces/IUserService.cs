using SportifyApi.DTOs;
using SportifyApi.Models;


namespace SportifyApi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(UserDto userDto, string password);
        Task<bool> UpdateUserAsync(int id, UserDto updatedUser);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetRawUserByEmailAsync(string email);


    }
}
