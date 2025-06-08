using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto, string password)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                UserType = string.IsNullOrWhiteSpace(userDto.UserType) ? "user" : userDto.UserType
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create empty profile
            var profile = new Profile
            {
                UserId = user.UserId,
                ProfilePicture = null,
                Location = null,
                Interests = null,
                FavoriteSports = null,
                Availability = null,
                Bio = null,
                PhoneNumber = null,
                SocialMediaLink = null,
                Gender = null,
                Age = null
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            userDto.UserId = user.UserId;
            userDto.UserType = user.UserType;
            return userDto;
        }



        public async Task<bool> UpdateUserAsync(int id, UserDto updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Optional: create empty profile for the new user
            var profile = new Profile
            {
                UserId = user.UserId,
                ProfilePicture = null,
                Location = null,
                Interests = null,
                FavoriteSports = null,
                Availability = null,
                Bio = null,
                PhoneNumber = null,
                SocialMediaLink = null,
                Gender = null,
                Age = null
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}