using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IFacilityService
    {
        Task<FacilitiesResponseDto?> GetFacilitiesAsync();

        Task<FacilityDto?> GetFacilityByIdAsync(Guid id);

        Task<FacilityDto> CreateFacilityAsync(string code, string name, string address);

        Task<FacilityDto> UpdateFacilityAsync(Guid id, string code, string name, string address);

        Task<bool> FacilityExistsAsync(Guid id);
    }
}