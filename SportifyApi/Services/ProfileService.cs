using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfileDto>> GetAllProfilesAsync()
        {
            return await _context.Profiles
                .Select(p => new ProfileDto
                {
                    UserId = p.UserId,
                    Name = p.Name,
                    Email = p.Email,
                    ProfilePicture = p.ProfilePicture,
                    Description = p.Description
                }).ToListAsync();
        }

        public async Task<ProfileDto?> GetProfileByIdAsync(int userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            if (profile == null) return null;

            return new ProfileDto
            {
                UserId = profile.UserId,
                Name = profile.Name,
                Email = profile.Email,
                ProfilePicture = profile.ProfilePicture,
                Description = profile.Description
            };
        }

        public async Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto)
        {
            var profile = new Profile
            {
                UserId = profileDto.UserId,
                Name = profileDto.Name,
                Email = profileDto.Email,
                ProfilePicture = profileDto.ProfilePicture,
                Description = profileDto.Description
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profileDto;
        }

        public async Task<bool> UpdateProfileAsync(int userId, ProfileDto updatedProfile)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            if (profile == null) return false;

            profile.Name = updatedProfile.Name;
            profile.Email = updatedProfile.Email;
            profile.ProfilePicture = updatedProfile.ProfilePicture;
            profile.Description = updatedProfile.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProfileAsync(int userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            if (profile == null) return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}