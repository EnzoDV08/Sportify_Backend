using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IProfileService
    {
        Task<IEnumerable<ProfileDto>> GetAllProfilesAsync();
        Task<ProfileDto?> GetProfileByIdAsync(int userId);
        Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto);
        Task<bool> UpdateProfileAsync(int userId, ProfileUpdateDto updatedProfile);
        Task<bool> DeleteProfileAsync(int userId);
    }
}