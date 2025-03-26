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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto, password: "default123"); // replace with actual password logic later
            return CreatedAtAction(nameof(GetUser), new { id = result.UserId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto updatedUser)
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
    }
}
