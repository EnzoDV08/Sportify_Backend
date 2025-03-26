using SportifyApi.DTOs;

namespace SportifyApi.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminDto>> GetAllAdminsAsync();
        Task<AdminDto?> GetAdminByIdAsync(int id);
        Task<AdminDto> CreateAdminAsync(AdminDto adminDto);
        Task<bool> UpdateAdminAsync(int id, AdminDto updatedAdmin);
        Task<bool> DeleteAdminAsync(int id);
    }
}
