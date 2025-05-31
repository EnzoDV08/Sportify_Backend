// Services/OrganizationProfileService.cs
using Microsoft.EntityFrameworkCore;
using SportifyApi.Data;
using SportifyApi.DTOs;
using SportifyApi.Interfaces;
using SportifyApi.Models;

namespace SportifyApi.Services
{
    public class OrganizationProfileService : IOrganizationProfileService
    {
        private readonly AppDbContext _context;

        public OrganizationProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationProfileDto?> GetProfileByIdAsync(int orgId)
        {
            var profile = await _context.OrganizationProfiles.FindAsync(orgId);
            if (profile == null) return null;

            return new OrganizationProfileDto
            {
                OrganizationId = profile.OrganizationId,
                LogoUrl = profile.LogoUrl,
                Description = profile.Description,
                ContactNumber = profile.ContactNumber,
                Location = profile.Location
            };
        }

        public async Task<OrganizationProfileDto> CreateProfileAsync(OrganizationProfileDto dto)
        {
            var profile = new OrganizationProfile
            {
                OrganizationId = dto.OrganizationId,
                LogoUrl = dto.LogoUrl,
                Description = dto.Description,
                ContactNumber = dto.ContactNumber,
                Location = dto.Location
            };

            _context.OrganizationProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task<bool> UpdateProfileAsync(int orgId, OrganizationProfileDto updated)
        {
            var profile = await _context.OrganizationProfiles.FindAsync(orgId);
            if (profile == null) return false;

            profile.LogoUrl = updated.LogoUrl ?? profile.LogoUrl;
            profile.Description = updated.Description ?? profile.Description;
            profile.ContactNumber = updated.ContactNumber ?? profile.ContactNumber;
            profile.Location = updated.Location ?? profile.Location;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProfileAsync(int orgId)
        {
            var profile = await _context.OrganizationProfiles.FindAsync(orgId);
            if (profile == null) return false;

            _context.OrganizationProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
