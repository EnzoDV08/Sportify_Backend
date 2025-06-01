// Controllers/OrganizationsController.cs
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
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _orgService;
        private readonly AppDbContext _context;

        public OrganizationsController(IOrganizationService orgService, AppDbContext context)
        {
            _orgService = orgService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] OrganizationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Password is required.");

            var created = await _orgService.CreateAsync(dto, dto.Password!);

            // Automatically create empty organization profile
            var profile = new OrganizationProfileDto
            {
                OrganizationId = created.OrganizationId
            };
            await _context.OrganizationProfiles.AddAsync(new OrganizationProfile
            {
                OrganizationId = profile.OrganizationId,
                LogoUrl = null,
                Description = null,
                ContactNumber = null,
                Location = null
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = created.OrganizationId }, created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.Email == login.Email);
            if (org == null || !BCrypt.Net.BCrypt.Verify(login.Password, org.Password))
                return Unauthorized("Invalid email or password.");

            return Ok(new OrganizationDto
            {
                OrganizationId = org.OrganizationId,
                Name = org.Name,
                Email = org.Email,
                Website = org.Website,
                ContactPerson = org.ContactPerson
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var org = await _orgService.GetByIdAsync(id);
            return org == null ? NotFound() : Ok(org);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orgService.GetAllAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orgService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganization(int id, [FromBody] UpdateOrganizationDto dto)
        {
            var updated = await _orgService.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }
    }
}
