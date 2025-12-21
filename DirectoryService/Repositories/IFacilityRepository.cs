using DirectoryService.Models.Entities;

namespace DirectoryService.Repositories
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetFacilitiesAsync();

        Task<Facility?> GetFacilityByIdAsync(Guid facilityId);

        Task<Facility> CreateFacilityAsync(string code, string name, string address);

        Task<Facility> UpdateFacilityAsync(Guid facilityId, string code, string name, string address);

        Task<bool> DeleteFacilityAsync(Guid facilityId);

        Task<bool> FacilityExistsAsync(Guid facilityId);
    }
}
