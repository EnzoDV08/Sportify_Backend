using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IUserService
    {
        // Retrieves all users
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        // Gets users by ID
        Task<UserDto?> GetUserByIdAsync(int id);

        // Create a new user with plain text password (It will be hashed later)
        Task<UserDto> CreateUserAsync(UserDto userDto, string password); 

        // Update user by ID (May not be needed)
        Task<bool> UpdateUserAsync(int id, UserDto updatedUser);

        // Deletes a user
        Task<bool> DeleteUserAsync(int id);
    }
}
