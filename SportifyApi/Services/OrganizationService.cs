// Services/OrganizationService.cs
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _context;

        public OrganizationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganizationDto>> GetAllAsync()
        {
            return await _context.Organizations.Select(o => new OrganizationDto
            {
                OrganizationId = o.OrganizationId,
                Name = o.Name,
                Email = o.Email,
                Website = o.Website,
                ContactPerson = o.ContactPerson
            }).ToListAsync();
        }

        public async Task<OrganizationDto?> GetByIdAsync(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return null;

            return new OrganizationDto
            {
                OrganizationId = org.OrganizationId,
                Name = org.Name,
                Email = org.Email,
                Website = org.Website,
                ContactPerson = org.ContactPerson
            };
        }

        public async Task<OrganizationDto> CreateAsync(OrganizationDto dto, string password)
        {
            var org = new Organization
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Website = dto.Website,
                ContactPerson = dto.ContactPerson
            };

            _context.Organizations.Add(org);
            await _context.SaveChangesAsync();

            dto.OrganizationId = org.OrganizationId;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org == null) return false;

            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int orgId, UpdateOrganizationDto dto)
        {
            var org = await _context.Organizations.FindAsync(orgId);
            if (org == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                org.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                org.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                org.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            if (!string.IsNullOrWhiteSpace(dto.Website))
                org.Website = dto.Website;

            if (!string.IsNullOrWhiteSpace(dto.ContactPerson))
                org.ContactPerson = dto.ContactPerson;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
