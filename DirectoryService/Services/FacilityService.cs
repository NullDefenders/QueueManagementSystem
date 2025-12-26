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

            public async Task<FacilityDto?> GetFacilityByIdAsync(Guid id)
            {
                try
                {
                    var facility = await _facilityRepository.GetFacilityByIdAsync(id);

                    if (facility == null)
                    {
                        return null;
                    }

                    return new FacilityDto(
                        id: facility.Id,
                        code: facility.Code,
                        name: facility.Name,
                        address: facility.Address
                    );
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            public async Task<FacilityDto> CreateFacilityAsync(string code, string name, string address)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(code))
                        throw new ArgumentException("Код учреждения не может быть пустым");

                    if (string.IsNullOrWhiteSpace(name))
                        throw new ArgumentException("Название учреждения не может быть пустым");

                    var facility = await _facilityRepository.CreateFacilityAsync(
                        code.Trim(),
                        name.Trim(),
                        address?.Trim() ?? string.Empty);

                    return new FacilityDto(
                        id: facility.Id,
                        code: facility.Code,
                        name: facility.Name,
                        address: facility.Address
                    );
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("уже существует"))
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            public async Task<FacilityDto> UpdateFacilityAsync(Guid id, string code, string name, string address)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(code))
                        throw new ArgumentException("Код учреждения не может быть пустым");

                    if (string.IsNullOrWhiteSpace(name))
                        throw new ArgumentException("Название учреждения не может быть пустым");

                    var facility = await _facilityRepository.UpdateFacilityAsync(
                        id,
                        code.Trim(),
                        name.Trim(),
                        address?.Trim() ?? string.Empty);

                    return new FacilityDto(
                        id: facility.Id,
                        code: facility.Code,
                        name: facility.Name,
                        address: facility.Address
                    );
                }
                catch (KeyNotFoundException ex)
                {
                    throw;
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("уже существует"))
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            public async Task<bool> DeleteFacilityAsync(Guid id)
            {
                try
                {
                    var facilityExists = await _facilityRepository.FacilityExistsAsync(id);
                    if (!facilityExists)
                    {
                        throw new KeyNotFoundException($"Учреждение с ID {id} не найдено.");
                    }

                    var result = await _facilityRepository.DeleteFacilityAsync(id);
                    return result;
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("нельзя удалить"))
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при удалении учреждения: {ex.Message}", ex);
                }
            }

            public async Task<bool> FacilityExistsAsync(Guid id)
            {
                try
                {
                    return await _facilityRepository.FacilityExistsAsync(id);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
