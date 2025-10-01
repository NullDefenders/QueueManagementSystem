using DirectoryService.Models.DTOs;
using DirectoryService.Repositories;

namespace DirectoryService.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IFacilityRepository _facilityRepository;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IFacilityRepository facilityRepository)
        {
            _departmentRepository = departmentRepository;
            _facilityRepository = facilityRepository;
        }

        public async Task<FacilityDepartmentsResponseDto?> GetFacilityDepartmentsAsync(Guid facilityId)
        {
            if (!await _facilityRepository.FacilityExistsAsync(facilityId))
                return null;

            var departments = await _departmentRepository.GetFacilityDepartmentsAsync(facilityId);

            return new FacilityDepartmentsResponseDto(
                departments.Select(d => new FacilityDepartmentsResponseDto.DepartmentDto(
                    id: d.Id,
                    code: d.Code,
                    name: d.Name,
                    address: d.Address,
                    allowScheduledAppointments: d.AllowScheduledAppointments
                )).ToList()
            );
        }

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<DepartmentSchedulesResponseDto?> GetDepartmentSchedulesAsync(Guid departmentId)
        {
            if (!await _departmentRepository.DepartmentExistsAsync(departmentId))
                return null;

            var schedules = await _departmentRepository.GetDepartmentSchedulesAsync(departmentId);

            return new DepartmentSchedulesResponseDto
            {
               Schedules = schedules.Select(s => new DepartmentSchedulesResponseDto.ScheduleDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    WorkDayStart = s.WorkDayStart,
                    WorkDayEnd = s.WorkDayEnd,
                    NonWorkingPeriods = s.NonWorkingPeriods
                        .Select(np => new DepartmentSchedulesResponseDto.NonWorkingPeriodDto
                        {
                            Id = np.Id,
                            DayOfWeek = np.DayOfWeek,
                            StartTime = np.StartTime,
                            EndTime = np.EndTime
                        })
                        .ToList()
                }).ToList()
            };
        }

        public async Task<DepartmentWorkplacesResponseDto?> GetDepartmentWorkplacesAsync(Guid departmentId)
        {
            if (!await _departmentRepository.DepartmentExistsAsync(departmentId))
                return null;

            var workplaces = await _departmentRepository.GetDepartmentWorkplacesAsync(departmentId);

            return new DepartmentWorkplacesResponseDto(
                workplaces.Select(w => new DepartmentWorkplacesResponseDto.WorkplaceDto(
                    w.Id,
                    w.Code,
                    w.Name,
                    w.ServiceCategories
                        .SelectMany(c => c.Services)
                        .Select(s => new DepartmentWorkplacesResponseDto.ServiceInfoDto(
                            s.ServiceCategory.Id,
                            s.ServiceCategory.Code,
                            s.ServiceCategory.Name,
                            s.ServiceCategory.Prefix,
                            s.Id,
                            s.Code,
                            s.Name))
                        .ToList()
                )).ToList()
            );
        }

        public async Task<DepartmentServicesResponseDto?> GetDepartmentServicesAsync(Guid departmentId)
        {
            if (!await _departmentRepository.DepartmentExistsAsync(departmentId))
                return null;

            var services = await _departmentRepository.GetDepartmentServicesAsync(departmentId);

            return new DepartmentServicesResponseDto(
                services.Select(service => new DepartmentServicesResponseDto.ServiceInfoDto(
                    categoryId: service.ServiceCategory.Id,
                    categoryCode: service.ServiceCategory.Code,
                    categoryName: service.ServiceCategory.Name,
                    categoryPrefix: service.ServiceCategory.Prefix,
                    serviceId: service.Id,
                    serviceCode: service.Code,
                    serviceName: service.Name
                )).ToList()
                );
        }
    }
}
