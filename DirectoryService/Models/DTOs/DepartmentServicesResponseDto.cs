using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об услугах подразделения
    /// </summary>
    public class DepartmentServicesResponseDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public DepartmentServicesResponseDto() { }

        /// <summary>
        /// Конструктор с параметрами (для сервисного слоя)
        /// </summary>
        public DepartmentServicesResponseDto(List<DepartmentServiceInfoDto> services)
        {
            Services = services;
        }

        /// <summary>
        /// Список оказываемых услуг
        /// </summary>
        [Display(Name = "Услуги")]
        public List<DepartmentServiceInfoDto> Services { get; set; } = new List<DepartmentServiceInfoDto>();

        /// <summary>
        /// DTO с информацией об услугах подразделения
        /// </summary>
        public class DepartmentServiceInfoDto
        {
            /// <summary>
            /// Конструктор по умолчанию (для Swagger)
            /// </summary>
            public DepartmentServiceInfoDto() { }

            public DepartmentServiceInfoDto(
                Guid categoryId,
                string categoryCode,
                string categoryName,
                string categoryPrefix,
                Guid serviceId,
                string serviceCode,
                string serviceName)
            {
                CategoryId = categoryId;
                CategoryCode = categoryCode;
                CategoryName = categoryName;
                CategoryPrefix = categoryPrefix;
                ServiceId = serviceId;
                ServiceCode = serviceCode;
                ServiceName = serviceName;
            }

            [Display(Name = "ID категории услуги")]
            public Guid CategoryId { get; set; }

            [Display(Name = "Код категории услуги")]
            public string CategoryCode { get; set; } = string.Empty;

            [Display(Name = "Название категории услуги")]
            public string CategoryName { get; set; } = string.Empty;

            [Display(Name = "Префикс категории услуги")]
            public string CategoryPrefix { get; set; } = string.Empty;

            [Display(Name = "ID услуги")]
            public Guid ServiceId { get; set; }

            [Display(Name = "Код услуги")]
            public string ServiceCode { get; set; } = string.Empty;

            [Display(Name = "Название услуги")]
            public string ServiceName { get; set; } = string.Empty;
        }
    }
}