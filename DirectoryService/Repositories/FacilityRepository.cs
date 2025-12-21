using DirectoryService.Data;
using DirectoryService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        public async Task<Facility?> GetFacilityByIdAsync(Guid facilityId)
        {
            return await _dbContext.Facilities
                .FirstOrDefaultAsync(f => f.Id == facilityId && f.IsActive);
        }

        public async Task<Facility> CreateFacilityAsync(string code, string name, string address)
        {
            var existingFacility = await _dbContext.Facilities
                .FirstOrDefaultAsync(f => f.Code == code && f.IsActive);

            if (existingFacility != null)
            {
                throw new InvalidOperationException($"Учреждение с кодом '{code}' уже существует.");
            }

            var facility = new Facility
            {
                Id = Guid.NewGuid(),
                Code = code.Trim(),
                Name = name.Trim(),
                Address = address?.Trim() ?? string.Empty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _dbContext.Facilities.AddAsync(facility);
            await _dbContext.SaveChangesAsync();

            return facility;
        }

        public async Task<Facility> UpdateFacilityAsync(Guid facilityId, string code, string name, string address)
        {
            var facility = await _dbContext.Facilities
                .FirstOrDefaultAsync(f => f.Id == facilityId && f.IsActive);

            if (facility == null)
            {
                throw new KeyNotFoundException($"Учреждение с ID {facilityId} не найдено.");
            }

            var duplicateCode = await _dbContext.Facilities
                .FirstOrDefaultAsync(f => f.Code == code && f.Id != facilityId && f.IsActive);

            if (duplicateCode != null)
            {
                throw new InvalidOperationException($"Учреждение с кодом '{code}' уже существует.");
            }

            facility.Code = code.Trim();
            facility.Name = name.Trim();
            facility.Address = address?.Trim() ?? string.Empty;

            _dbContext.Facilities.Update(facility);
            await _dbContext.SaveChangesAsync();

            return facility;
        }

        public async Task<bool> DeleteFacilityAsync(Guid facilityId)
        {
            var facility = await _dbContext.Facilities
                .FirstOrDefaultAsync(f => f.Id == facilityId && f.IsActive);

            if (facility == null)
            {
                return false;
            }

            var hasDepartments = await _dbContext.Departments
                .AnyAsync(d => d.FacilityId == facilityId && d.IsActive);

            if (hasDepartments)
            {
                throw new InvalidOperationException(
                    "Нельзя удалить учреждение, у которого есть активные подразделения. " +
                    "Сначала удалите или деактивируйте все подразделения.");
            }

            facility.IsActive = false;
            facility.DeletedAt = DateTime.UtcNow;

            _dbContext.Facilities.Update(facility);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FacilityExistsAsync(Guid facilityId)
        {
            return await _dbContext.Facilities.AnyAsync(f => f.Id == facilityId);
        }
    }
}
