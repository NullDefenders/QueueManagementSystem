using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных подразделениях учреждения
    /// </summary>
    public class FacilityDepartmentsResponseDto
    {
        /// <summary>
        /// Список активных подразделений учреждения
        /// </summary>
        [Display(Name = "Список активных учреждений")]
        public List<DepartmentDto> Departments { get; }

        public FacilityDepartmentsResponseDto(List<DepartmentDto> departments)
        {
            Departments = departments;
        }

        public class DepartmentDto
        {
            [Display(Name = "ID подразделения")]
            public Guid Id { get; }

            [Display(Name = "Код подразделения")]
            public string Code { get; }

            [Display(Name = "Название подразделения")]
            public string Name { get; }

            [Display(Name = "Адрес подразделения")]
            public string Address { get; }

            [Display(Name = "Разрешена ли предварительная запись")]
            public bool AllowScheduledAppointments { get; }

            public DepartmentDto(
                Guid id,
                string code,
                string name,
                string address,
                bool allowScheduledAppointments)
            {
                Id = id;
                Code = code;
                Name = name;
                Address = address;
                AllowScheduledAppointments = allowScheduledAppointments;
            }
        }
    }
}
