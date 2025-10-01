using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IDepartmentService
    {
        Task<FacilityDepartmentsResponseDto?> GetFacilityDepartmentsAsync(Guid facilityId);

        Task<DepartmentSchedulesResponseDto?> GetDepartmentSchedulesAsync(Guid departmentId);

        Task<DepartmentWorkplacesResponseDto?> GetDepartmentWorkplacesAsync(Guid departmentId);

        Task<DepartmentServicesResponseDto?> GetDepartmentServicesAsync(Guid departmentId);
    }
}
