using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных окнах подразделения
    /// </summary>
    public class DepartmentWorkplacesResponseDto
    {
        /// <summary>
        /// Список рабочих мест подразделения
        /// </summary>
        [Display(Name = "Список окон подразделения")]
<<<<<<< HEAD
        public List<WorkplaceDto> Workplaces { get; }
=======
        public List<WorkplaceDto> Workplaces { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

        public DepartmentWorkplacesResponseDto(List<WorkplaceDto> workplaces)
        {
            Workplaces = workplaces;
        }

        /// <summary>
        /// DTO с информацией об окне
        /// </summary>
        public class WorkplaceDto
        {
            /// <summary>
            /// Уникальный идентификатор окна
            /// </summary>
            [Display(Name = "Уникальный идентификатор окна")]
<<<<<<< HEAD
            public Guid Id { get; }
=======
            public Guid Id { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

            /// <summary>
            /// Уникальный код окна
            /// </summary>
            [Display(Name = "Уникальный код окна")]
<<<<<<< HEAD
            public string Code { get; }
=======
            public string Code { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

            /// <summary>
            /// Название окна
            /// </summary>
            [Display(Name = "Название окна")]
<<<<<<< HEAD
            public string Name { get; }
=======
            public string Name { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

            /// <summary>
            /// Список услуг, оказываемых окном
            /// </summary>
            [Display(Name = "Услуги, оказываемые окном")]
<<<<<<< HEAD
            public List<ServiceInfoDto> Services { get; }
=======
            public List<ServiceInfoDto> Services { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

            public WorkplaceDto(Guid id, string code, string name, List<ServiceInfoDto> services)
            {
                Id = id;
                Code = code;
                Name = name;
                Services = services;
            }
        }

        /// <summary>
        /// DTO с информацией об услугах
        /// </summary>
        public class ServiceInfoDto
        {
            [Display(Name = "ID категории услуги")]
<<<<<<< HEAD
            public Guid CategoryId { get; }

            [Display(Name = "Код категории услуги")]
            public string CategoryCode { get; }

            [Display(Name = "Название категории услуги")]
            public string CategoryName { get; }

            [Display(Name = "ID услуги")]
            public Guid ServiceId { get; }

            [Display(Name = "Код услуги")]
            public string ServiceCode { get; }

            [Display(Name = "Название услуги")]
            public string ServiceName { get; }
=======
            public Guid CategoryId { get; set; }

            [Display(Name = "Код категории услуги")]
            public string CategoryCode { get; set; }

            [Display(Name = "Название категории услуги")]
            public string CategoryName { get; set; }

            [Display(Name = "ID услуги")]
            public Guid ServiceId { get; set; }

            [Display(Name = "Код услуги")]
            public string ServiceCode { get; set; }

            [Display(Name = "Название услуги")]
            public string ServiceName { get; set; }
>>>>>>> d5e60c05f59b8083419873ace83c49b616cf056a

            public ServiceInfoDto(
                Guid categoryId,
                string categoryCode,
                string categoryName,
                Guid serviceId,
                string serviceCode,
                string serviceName)
            {
                CategoryId = categoryId;
                CategoryCode = categoryCode;
                CategoryName = categoryName;
                ServiceId = serviceId;
                ServiceCode = serviceCode;
                ServiceName = serviceName;
            }
        }
    }
}