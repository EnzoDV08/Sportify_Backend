using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;
using OtpNet;
using QRCoder;
using System.IO;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UsersController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsersByName([FromQuery] string name)
        {
            var users = await _context.Users
                .Where(u => u.Name.ToLower().Contains(name.ToLower())) // 👈 case-insensitive
                .Select(u => new { u.UserId, u.Name })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Password is required.");
            }

            var createdUser = await _userService.CreateUserAsync(userDto, userDto.Password!);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto updatedUser)
        {
            var success = await _userService.UpdateUserAsync(id, updatedUser);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            // 🔒 If 2FA is enabled, return a special response
            if (user.IsTwoFactorEnabled)
            {
                return Ok(new
                {
                    Requires2FA = true,
                    UserId = user.UserId,
                    Email = user.Email
                });
            }

            // ✅ Normal login flow
            var userDto = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };

            return Ok(userDto);
        }


        // Add this to UsersController.cs

        [HttpPut("{id}/toggle-2fa")]
        public async Task<IActionResult> ToggleTwoFactor(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.IsTwoFactorEnabled = !user.IsTwoFactorEnabled;
            await _context.SaveChangesAsync();

            return Ok(new { isTwoFactorEnabled = user.IsTwoFactorEnabled });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2faDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || string.IsNullOrEmpty(user.TwoFactorSecret))
                return Unauthorized("User not found or 2FA not configured.");

            // ✅ Even if IsTwoFactorEnabled is false, allow verification if a secret is still present

            // Decode the Base32 secret and initialize the TOTP
            var secretBytes = Base32Encoding.ToBytes(user.TwoFactorSecret);
            var totp = new Totp(secretBytes);

            // Validate the 6-digit code
            bool isValid = totp.VerifyTotp(dto.Code, out long _, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (!isValid)
                return Unauthorized("Invalid 2FA code.");

            return Ok("2FA verified successfully.");
        }


        [HttpPost("{id}/generate-2fa")]
        public async Task<ActionResult<TwoFactorSetupDto>> GenerateTwoFactorSetup(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            // 1. Generate a 20-byte random secret
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(key);

            // 2. Store the secret in the database
            user.TwoFactorSecret = base32Secret;
            await _context.SaveChangesAsync();

            // 3. Create QR code URI compatible with Google Authenticator
            string issuer = "Sportify";
            string label = $"{issuer}:{user.Email}";
            string otpAuthUri = $"otpauth://totp/{label}?secret={base32Secret}&issuer={issuer}";

            // 4. Generate QR code image as Base64
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            byte[] pngBytes = qrCode.GetGraphic(20); // Adjust pixel size if needed
            string pngBase64 = Convert.ToBase64String(pngBytes);
            string imageUrl = $"data:image/png;base64,{pngBase64}"; ;


            // 5. Return QR image and manual code
            return Ok(new TwoFactorSetupDto
            {
                QrCodeImageUrl = imageUrl,
                ManualEntryKey = base32Secret
            });
        }
        
        [HttpPost("disable-2fa")]
        public async Task<IActionResult> Disable2FA([FromBody] Verify2faDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || string.IsNullOrEmpty(user.TwoFactorSecret))
                return Unauthorized("User not found or 2FA is not configured.");

            var secretBytes = Base32Encoding.ToBytes(user.TwoFactorSecret);
            var totp = new Totp(secretBytes);
            bool isValid = totp.VerifyTotp(dto.Code, out long _, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (!isValid)
                return Unauthorized("Invalid 2FA code.");

            // ✅ Disable and wipe secret
            user.IsTwoFactorEnabled = false;
            user.TwoFactorSecret = null;
            await _context.SaveChangesAsync();

            return Ok("2FA disabled successfully.");
        }


    }
}