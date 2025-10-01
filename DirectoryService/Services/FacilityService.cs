using DirectoryService.Models.DTOs;
using DirectoryService.Repositories;

namespace DirectoryService.Services
{
    namespace DirectoryService.Services
    {
        public class FacilityService : IFacilityService
        {
            private readonly IFacilityRepository _facilityRepository;

            public FacilityService(IFacilityRepository facilityRepository)
            {
                _facilityRepository = facilityRepository;
            }

            public async Task<FacilitiesResponseDto?> GetFacilitiesAsync()
            {
                var facilities = await _facilityRepository.GetFacilitiesAsync();

                return new FacilitiesResponseDto(
                    facilities.Select(f => new FacilityDto(
                        id: f.Id,
                        code: f.Code,
                        name: f.Name,
                        address: f.Address
                    )).ToList()
                );
            }
        }
    }
}
