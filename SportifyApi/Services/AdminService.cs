using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
        {
            return await _context.Admins
                .Select(a => new AdminDto
                {
                    AdminId = a.AdminId,
                    Name = a.Name,
                    Email = a.Email,
                    UserId = a.UserId
                }).ToListAsync();
        }

        public async Task<AdminDto?> GetAdminByIdAsync(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return null;

            return new AdminDto
            {
                AdminId = admin.AdminId,
                Name = admin.Name,
                Email = admin.Email,
                UserId = admin.UserId
            };
        }

        public async Task<AdminDto> CreateAdminAsync(AdminDto adminDto)
        {
            var admin = new Admin
            {
                Name = adminDto.Name,
                Email = adminDto.Email,
                UserId = adminDto.UserId
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            adminDto.AdminId = admin.AdminId;
            return adminDto;
        }

        public async Task<bool> UpdateAdminAsync(int id, AdminDto updatedAdmin)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return false;

            admin.Name = updatedAdmin.Name;
            admin.Email = updatedAdmin.Email;
            admin.UserId = updatedAdmin.UserId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return false;

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
