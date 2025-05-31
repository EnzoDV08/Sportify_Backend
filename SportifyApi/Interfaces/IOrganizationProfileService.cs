// Interfaces/IOrganizationProfileService.cs
using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IOrganizationProfileService
    {
        Task<OrganizationProfileDto?> GetProfileByIdAsync(int orgId);
        Task<OrganizationProfileDto> CreateProfileAsync(OrganizationProfileDto dto);
        Task<bool> UpdateProfileAsync(int orgId, OrganizationProfileDto updated);
        Task<bool> DeleteProfileAsync(int orgId);
    }
}
