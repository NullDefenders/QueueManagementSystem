using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IDepartmentService
    {
<<<<<<< HEAD
        Task<FacilityDepartmentsResponseDto?> GetFacilityDepartmentsAsync(Guid facilityId);

=======
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a
        Task<DepartmentSchedulesResponseDto?> GetDepartmentSchedulesAsync(Guid departmentId);

        Task<DepartmentWorkplacesResponseDto?> GetDepartmentWorkplacesAsync(Guid departmentId);

        Task<DepartmentServicesResponseDto?> GetDepartmentServicesAsync(Guid departmentId);
    }
}
