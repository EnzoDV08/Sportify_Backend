using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

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

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };

            return Ok(userDto);
        }
    }
}
