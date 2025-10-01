using DirectoryService.Data;
using DirectoryService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationContext _dbContext;

        public DepartmentRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Department>> GetFacilityDepartmentsAsync(Guid facilityId)
        {
            return await _dbContext.Departments
                .Where(d => d.FacilityId == facilityId && d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetDepartmentSchedulesAsync(Guid departmentId)
        {
            return await _dbContext.Schedules
                .Where(s => s.DepartmentId == departmentId && s.IsActive)
                .Include(s => s.NonWorkingPeriods)
                .ToListAsync();
        }

        public async Task<IEnumerable<Workplace>> GetDepartmentWorkplacesAsync(Guid departmentId)
        {
            return await _dbContext.Workplaces
                .Where(w => w.DepartmentId == departmentId && w.IsActive)
                .Include(w => w.Department)
                .Include(w => w.ServiceCategories)
                    .ThenInclude(c => c.Services)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetDepartmentServicesAsync(Guid departmentId)
        {
            return await _dbContext.Departments
                .Where(d => d.Id == departmentId)
                .SelectMany(d => d.ServiceCategories
                    .Where(c => c.IsActive)
                    .SelectMany(c => c.Services
                      .Where(s => s.IsActive)
                    )
                )
                .Include(s => s.ServiceCategory)
                .ToListAsync();
        }

        public async Task<bool> DepartmentExistsAsync(Guid departmentId)
        {
            return await _dbContext.Departments.AnyAsync(d => d.Id == departmentId);
        }  
    }
}
