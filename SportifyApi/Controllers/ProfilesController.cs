using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetProfiles()
        {
            return Ok(await _profileService.GetAllProfilesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDto>> GetProfile(int id)
        {
            var profile = await _profileService.GetProfileByIdAsync(id);
            return profile == null ? NotFound() : Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileDto>> CreateProfile(ProfileDto profileDto)
        {
            var created = await _profileService.CreateProfileAsync(profileDto);
            return CreatedAtAction(nameof(GetProfile), new { id = created.UserId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, ProfileDto updatedProfile)
        {
            var success = await _profileService.UpdateProfileAsync(id, updatedProfile);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var success = await _profileService.DeleteProfileAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}