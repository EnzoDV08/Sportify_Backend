using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyApi.DTOs;
using SportifyApi.Services;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using Google.Apis.Auth;
using SportifyApi.Data;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly GoogleAuthService _googleAuthService;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public AuthController(GoogleAuthService googleAuthService, IUserService userService, AppDbContext context)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _context = context;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenDto dto)
        {
            var payload = await _googleAuthService.VerifyTokenAsync(dto.Token);
            if (payload == null)
                return Unauthorized("Invalid Google token");

            var user = await _userService.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Name = payload.Name ?? "Google User",
                    Email = payload.Email,
                    Password = "", // Leave empty or label as "google"
                    UserType = "User"
                };

                // Save the user (and auto-create profile)
                user = await _userService.CreateUserAsync(user);

                // Update the profile with Google picture
                var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.UserId);
                if (profile != null)
                {
                    profile.ProfilePicture = payload.Picture;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new
            {
                user.UserId,
                user.UserType
            });
        }
    }
}
