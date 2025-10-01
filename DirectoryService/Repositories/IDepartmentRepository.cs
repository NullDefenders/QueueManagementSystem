using DirectoryService.Models.Entities;

namespace DirectoryService.Repositories
{
    public interface IDepartmentRepository
    {
<<<<<<< HEAD
        Task<IEnumerable<Department>> GetFacilityDepartmentsAsync(Guid facilityId);

=======
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a
        Task<IEnumerable<Schedule>> GetDepartmentSchedulesAsync(Guid departmentId);

        Task<IEnumerable<Workplace>> GetDepartmentWorkplacesAsync(Guid departmentId);

        Task<IEnumerable<Service>> GetDepartmentServicesAsync(Guid departmentId);

        Task<bool> DepartmentExistsAsync(Guid departmentId);
    }
}
