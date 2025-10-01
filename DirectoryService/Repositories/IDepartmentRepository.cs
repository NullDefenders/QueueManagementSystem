using DirectoryService.Models.Entities;

namespace DirectoryService.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetFacilityDepartmentsAsync(Guid facilityId);

        Task<IEnumerable<Schedule>> GetDepartmentSchedulesAsync(Guid departmentId);

        Task<IEnumerable<Workplace>> GetDepartmentWorkplacesAsync(Guid departmentId);

        Task<IEnumerable<Service>> GetDepartmentServicesAsync(Guid departmentId);

        Task<bool> DepartmentExistsAsync(Guid departmentId);
    }
}
