using DirectoryService.Data;
using DirectoryService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly ApplicationContext _dbContext;

        public FacilityRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Facility>> GetFacilitiesAsync()
        {
            return await _dbContext.Facilities
                .Where(f => f.IsActive)
                .ToListAsync();
        }

        public async Task<bool> FacilityExistsAsync(Guid facilityId)
        {
            return await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId);
        }
    }
}
