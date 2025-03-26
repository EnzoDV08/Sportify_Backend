using Microsoft.AspNetCore.Mvc;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;

namespace SportifyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAdmins()
        {
            return Ok(await _adminService.GetAllAdminsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetAdmin(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            return admin == null ? NotFound() : Ok(admin);
        }

        [HttpPost]
        public async Task<ActionResult<AdminDto>> CreateAdmin(AdminDto adminDto)
        {
            var created = await _adminService.CreateAdminAsync(adminDto);
            return CreatedAtAction(nameof(GetAdmin), new { id = created.AdminId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, AdminDto updatedAdmin)
        {
            var success = await _adminService.UpdateAdminAsync(id, updatedAdmin);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var success = await _adminService.DeleteAdminAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}