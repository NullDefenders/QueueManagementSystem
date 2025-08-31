using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IDepartmentService
    {
        Task<DepartmentSchedulesResponseDto?> GetDepartmentSchedulesAsync(Guid departmentId);

        Task<DepartmentWorkplacesResponseDto?> GetDepartmentWorkplacesAsync(Guid departmentId);

        Task<DepartmentServicesResponseDto?> GetDepartmentServicesAsync(Guid departmentId);
    }
}
