using DirectoryService.Models.Entities;

namespace DirectoryService.Repositories
{
    public interface IFacilityRepository
    {
        Task<IEnumerable<Facility>> GetFacilitiesAsync();

        Task<bool> FacilityExistsAsync(Guid facilityId);
    }
}
