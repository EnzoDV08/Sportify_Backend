using SportifyApi.DTOs;
using SportifyApi.Models;

namespace SportifyApi.Interfaces
{
    public interface IUserService
    {
        // Retrieves all users
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        // Gets user by ID
        Task<UserDto?> GetUserByIdAsync(int id);

        // Creates a new user using a DTO + password
        Task<UserDto> CreateUserAsync(UserDto userDto, string password);

        // Updates a user
        Task<bool> UpdateUserAsync(int id, UserDto updatedUser);

        // Deletes a user
        Task<bool> DeleteUserAsync(int id);

        // Google Sign-In helpers
        Task<User?> FindByEmailAsync(string email);           // For checking existing user
        Task<User> CreateUserAsync(User user);                // For creating user from raw model
    }
}
