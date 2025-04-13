using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/5
       
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            string password = userDto.Password ?? "default"; // In production, validate or hash this!
            var createdUser = await _userService.CreateUserAsync(userDto, password);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.UserId }, createdUser);
        }

        // PUT: api/users/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto updatedUser)
        {
            var success = await _userService.UpdateUserAsync(id, updatedUser);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/users/5
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
