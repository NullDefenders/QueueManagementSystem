using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об услугах подразделения
    /// </summary>
    /// 
    public class DepartmentServicesResponseDto
    {
        /// <summary>
        /// Список оказываемых услуг
        /// </summary>
        [Display(Name = "Услуги")]
        public List<ServiceInfoDto> Services { get; }

        public DepartmentServicesResponseDto(List<ServiceInfoDto> services)
        {
            Services = services;
        }

        /// <summary>
        /// DTO с информацией об услугах подразделения
        /// </summary>
        public class ServiceInfoDto
        {
            [Display(Name = "ID категории услуги")]
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
