using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных подразделениях учреждения
    /// </summary>
    public class FacilityDepartmentsResponseDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public FacilityDepartmentsResponseDto() { }

        /// <summary>
        /// Конструктор с параметрами (для сервисного слоя)
        /// </summary>
        public FacilityDepartmentsResponseDto(List<DepartmentDto> departments)
        {
            Departments = departments;
        }

        /// <summary>
        /// Список активных подразделений учреждения
        /// </summary>
        [Display(Name = "Список активных учреждений")]
        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();

        public class DepartmentDto
        {
            /// <summary>
            /// Конструктор по умолчанию (для Swagger)
            /// </summary>
            public DepartmentDto() { }

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

            [Display(Name = "ID подразделения")]
            public Guid Id { get; set; }

            [Display(Name = "Код подразделения")]
            public string Code { get; set; } = string.Empty;

            [Display(Name = "Название подразделения")]
            public string Name { get; set; } = string.Empty;

            [Display(Name = "Адрес подразделения")]
            public string Address { get; set; } = string.Empty;

            [Display(Name = "Разрешена ли предварительная запись")]
            public bool AllowScheduledAppointments { get; set; }
        }
    }
}