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

        // Create a new user with plain text password (It will be hashed later)
        Task<UserDto> CreateUserAsync(UserDto userDto, string password); 

        // Update user by ID (May not be needed)
        Task<bool> UpdateUserAsync(int id, UserDto updatedUser);

        // Deletes a user
        Task<bool> DeleteUserAsync(int id);

        // Google Sign-In helpers
        Task<User?> FindByEmailAsync(string email);           // For checking existing user
        Task<User> CreateUserAsync(User user);                // For creating user from raw model
    }
}
