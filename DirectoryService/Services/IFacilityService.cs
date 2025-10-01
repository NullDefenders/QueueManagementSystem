using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IFacilityService
    {
        Task<FacilitiesResponseDto?> GetFacilitiesAsync();
    }
}
