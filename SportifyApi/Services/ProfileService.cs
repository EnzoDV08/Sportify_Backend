using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using BCrypt.Net; 

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
            return await _context.Profiles //access the Profiles table
                .Select(profile => new ProfileDto
                {
                    UserId = profile.UserId,
                    ProfilePicture = profile.ProfilePicture,
                    Location = profile.Location,
                    Interests = profile.Interests,
                    FavoriteSports = profile.FavoriteSports,
                    Availability = profile.Availability,
                    Bio = profile.Bio,
                    PhoneNumber = profile.PhoneNumber,
                    SocialMediaLink = profile.SocialMediaLink,
                    Gender = profile.Gender,
                    Age = profile.Age
                })
                .ToListAsync();
        }

        public async Task<ProfileDto?> GetProfileByIdAsync(int userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            var user = await _context.Users.FindAsync(userId);

            if (profile == null || user == null)
                return null;

            return new ProfileDto
            {
                UserId = user.UserId,
                ProfilePicture = profile.ProfilePicture,
                Location = profile.Location,
                Interests = profile.Interests,
                FavoriteSports = profile.FavoriteSports,
                Availability = profile.Availability,
                Bio = profile.Bio,
                PhoneNumber = profile.PhoneNumber,
                SocialMediaLink = profile.SocialMediaLink,
                Gender = profile.Gender,
                Age = profile.Age
            };
        }

        public async Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto)
        {
            var profile = new Profile
            {
                UserId = profileDto.UserId,
                ProfilePicture = profileDto.ProfilePicture,
                Location = profileDto.Location,
                Interests = profileDto.Interests,
                FavoriteSports = profileDto.FavoriteSports,
                Availability = profileDto.Availability,
                Bio = profileDto.Bio,
                PhoneNumber = profileDto.PhoneNumber,
                SocialMediaLink = profileDto.SocialMediaLink,
                Gender = profileDto.Gender,
                Age = profileDto.Age
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profileDto;
        }
        
        // Allows Partial Updates
        public async Task<bool> UpdateProfileAsync(int userId, ProfileUpdateDto updatedProfile)
{
            var user = await _context.Users.FindAsync(userId); // Find the user by ID
            var profile = await _context.Profiles.FindAsync(userId); // Find the profile by user ID

            // Returns false if it cant find the user or profile
            if (user == null || profile == null)
                return false;


            // User table updates
            if (!string.IsNullOrWhiteSpace(updatedProfile.Name)) // Checks id string is not null or whitespace before it continues
            {
                user.Name = updatedProfile.Name;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Email))
            {
                user.Email = updatedProfile.Email;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updatedProfile.Password);
            }

            // Profile table updates
            if (!string.IsNullOrWhiteSpace(updatedProfile.ProfilePicture))
            {
                profile.ProfilePicture = updatedProfile.ProfilePicture;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Location))
            {
                profile.Location = updatedProfile.Location;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Interests))
            {
                profile.Interests = updatedProfile.Interests;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.FavoriteSports))
            {
                profile.FavoriteSports = updatedProfile.FavoriteSports;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Availability))
            {
                profile.Availability = updatedProfile.Availability;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Bio))
            {
                profile.Bio = updatedProfile.Bio;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.PhoneNumber))
            {
                profile.PhoneNumber = updatedProfile.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.SocialMediaLink))
            {
                profile.SocialMediaLink = updatedProfile.SocialMediaLink;
            }

            if (!string.IsNullOrWhiteSpace(updatedProfile.Gender))
            {
                profile.Gender = updatedProfile.Gender;
            }

            if (updatedProfile.Age.HasValue)
            {
                profile.Age = updatedProfile.Age.Value;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteProfileAsync(int userId)
        {
            var profile = await _context.Profiles.FindAsync(userId);
            if (profile == null)
                return false;

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}