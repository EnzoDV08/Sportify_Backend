using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using Microsoft.AspNetCore.Identity;

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
                    Email = u.Email,
                    UserType = u.UserType
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
                Email = user.Email,
                UserType = user.UserType
            };
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto, string password)
        {
            var passwordHasher = new PasswordHasher<User>();

            // Use provided userType or default to "user"
            var isCompany = userDto.UserType?.ToLower() == "company";

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                UserType = isCompany ? "company" : "user"
            };

            user.Password = passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var profile = new Profile
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Description = isCompany ? "Pending approval by admin" : "New user",
                ProfilePicture = ""
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

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, updatedUser.Password);
            }

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

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = user.UserType
            };
        }

        public async Task<User?> GetRawUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
