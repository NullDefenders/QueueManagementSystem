using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных учреждениях
    /// </summary>
    public class FacilitiesResponseDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public FacilitiesResponseDto() { }

        /// <summary>
        /// Конструктор с параметрами (для сервисного слоя)
        /// </summary>
        public FacilitiesResponseDto(List<FacilityDto> facilities)
        {
            Facilities = facilities;
        }

        /// <summary>
        /// Список активных учреждений
        /// </summary>
        [Display(Name = "Список активных учреждений")]
        public List<FacilityDto> Facilities { get; set; } = new List<FacilityDto>();
    }

    public class FacilityDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public FacilityDto() { }

        public FacilityDto(
            Guid id,
            string code,
            string name,
            string address)
        {
            Id = id;
            Code = code;
            Name = name;
            Address = address;
        }

        [Display(Name = "ID учреждения")]
        public Guid Id { get; set; }

        [Display(Name = "Код учреждения")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Название учреждения")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Адрес учреждения")]
        public string Address { get; set; } = string.Empty;
    }
}