// Interfaces/IOrganizationService.cs
using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationDto>> GetAllAsync();
        Task<OrganizationDto?> GetByIdAsync(int id);
        Task<OrganizationDto> CreateAsync(OrganizationDto dto, string password);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int orgId, UpdateOrganizationDto dto);

    }
}
