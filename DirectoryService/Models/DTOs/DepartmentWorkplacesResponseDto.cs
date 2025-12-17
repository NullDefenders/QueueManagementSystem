using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных окнах подразделения
    /// </summary>
    public class DepartmentWorkplacesResponseDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public DepartmentWorkplacesResponseDto() { }

        /// <summary>
        /// Конструктор с параметрами (для сервисного слоя)
        /// </summary>
        public DepartmentWorkplacesResponseDto(List<WorkplaceDto> workplaces)
        {
            Workplaces = workplaces;
        }

        /// <summary>
        /// Список рабочих мест подразделения
        /// </summary>
        [Display(Name = "Список окон подразделения")]
        public List<WorkplaceDto> Workplaces { get; set; } = new List<WorkplaceDto>();

        /// <summary>
        /// DTO с информацией об окне
        /// </summary>
        public class WorkplaceDto
        {
            /// <summary>
            /// Конструктор по умолчанию (для Swagger)
            /// </summary>
            public WorkplaceDto() { }

            public WorkplaceDto(Guid id, string code, string name, List<WorkplaceServiceInfoDto> services)
            {
                Id = id;
                Code = code;
                Name = name;
                Services = services;
            }

            /// <summary>
            /// Уникальный идентификатор окна
            /// </summary>
            [Display(Name = "Уникальный идентификатор окна")]
            public Guid Id { get; set; }

            /// <summary>
            /// Уникальный код окна
            /// </summary>
            [Display(Name = "Уникальный код окна")]
            public string Code { get; set; } = string.Empty;

            /// <summary>
            /// Название окна
            /// </summary>
            [Display(Name = "Название окна")]
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Список услуг, оказываемых окном
            /// </summary>
            [Display(Name = "Услуги, оказываемые окном")]
            public List<WorkplaceServiceInfoDto> Services { get; set; } = new List<WorkplaceServiceInfoDto>();
        }

        /// <summary>
        /// DTO с информацией об услугах
        /// </summary>
        public class WorkplaceServiceInfoDto
        {
            /// <summary>
            /// Конструктор по умолчанию (для Swagger)
            /// </summary>
            public WorkplaceServiceInfoDto() { }

            public WorkplaceServiceInfoDto(
                Guid categoryId,
                string categoryCode,
                string categoryName,
                string prefix,
                Guid serviceId,
                string serviceCode,
                string serviceName)
            {
                CategoryId = categoryId;
                CategoryCode = categoryCode;
                CategoryName = categoryName;
                CategoryPrefix = prefix;
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

            [Display(Name = "Префис категории услуги")]
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