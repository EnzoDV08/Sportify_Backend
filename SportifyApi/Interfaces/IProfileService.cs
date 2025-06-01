using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    // Interface for handling logic related to user profiles
    public interface IProfileService
    {   
        // Fetches all profiles
        Task<IEnumerable<ProfileDto>> GetAllProfilesAsync();

        // Gets Profile by ID
        Task<ProfileDto?> GetProfileByIdAsync(int userId);

        // Creates a new profile (May not be needed)
        Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto);

        // Update profile by user_ID 
        Task<bool> UpdateProfileAsync(int userId, ProfileUpdateDto updatedProfile);

        // Deletes a profile by user_ID (May not be needed)
        Task<bool> DeleteProfileAsync(int userId);
    }
}